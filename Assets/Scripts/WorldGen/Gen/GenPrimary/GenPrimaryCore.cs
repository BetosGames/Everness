using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;
using static GenPrimaryCore;
using static GenPrimaryCore.Sheet;

public class GenPrimaryCore: MonoBehaviour
{
    [Header("Preview")]
    public bool previewDisplayOnStart;
    public int previewSeed;
    public Vector2Int previewOffset;
    public int previewWidth;
    public int previewHeight;
    public int previewOnlyValue;
    [Header("Result Editor")]
    public string forPlanetID;
    public float biomeScale;
    public int minimumBiomeSize;
    public int biomeBlend;
    public AnimationCurve biomeBlendCurve;
    public int waterLevel;
    public int[] liquidValues;
    public int bottomLevel;
    public Biome[] biomes;
    public GlobalSheet[] globalSheets;
    public BiomeRequirement[] biomeRequirements;

    //For Preview Display (WARNING Preview takes a lot of juice!)

    private MapDisplayer2D mapDisplayer;

    private void Start()
    {
        mapDisplayer = GetComponentInChildren<MapDisplayer2D>();
        if (previewDisplayOnStart) StartCoroutine(PreviewLoop());
    }

    private IEnumerator PreviewLoop()
    {
        bool doNextGen = false;
        StartCoroutine(CreateMap(previewSeed, previewWidth, previewHeight, previewOffset, waterLevel, liquidValues, bottomLevel, biomes, biomeRequirements, biomeScale, minimumBiomeSize, biomeBlend, biomeBlendCurve, globalSheets, previewOnlyValue != -1 ? previewOnlyValue : null,
            (map) =>
            {
                mapDisplayer.Display(map);
                doNextGen = true;
            }));
        yield return new WaitUntil(() => doNextGen == true);
        StartCoroutine(PreviewLoop());
    }

    public void CreateMapBasedOnEditorSettings(int seed, int width, int height, Vector2Int offset, int? onlyValue, System.Action<int[,]> toDoWithMap)
    {
        StartCoroutine(CreateMap(seed, width, height, offset, waterLevel, liquidValues, bottomLevel, biomes, biomeRequirements, biomeScale, minimumBiomeSize, biomeBlend, biomeBlendCurve, globalSheets, onlyValue,
            (map) =>
            {
                toDoWithMap(map);
            }));
    }

    //The Main Primary Gen Map Generator

    private int generatedBiomeMaps = 0;
    private int requiredBiomeMaps = 0;

    private IEnumerator CreateMap(int seed, int width, int height, Vector2Int globalOffset, int waterLevel, int[] liquidValues, int bottomLevel, Biome[] biomes, BiomeRequirement[] biomeRequirements, float biomeScale, int minimumBiomeSize, int biomeBlend, AnimationCurve biomeBlendCurve, GlobalSheet[] globalSheets, int? onlyValue, System.Action<int[,]> toDoWithMap)
    {
        //System.Diagnostics.Stopwatch runtimeStopwatch = new System.Diagnostics.Stopwatch();
        //runtimeStopwatch.Start();

        float biomeBlendFalloff = biomeBlend * 0.61f;

        globalOffset.y *= -1;

        int[,] finalMap = new int[width,height];

        System.Random random = new System.Random(seed);
        System.Random sheetRandom = new System.Random(seed + random.Next());


        //Generating Noise Maps

        int[] biomeMap = GenerateBiomeMap(random, width + (biomeBlend * 4) + (minimumBiomeSize * 4), globalOffset.x - (biomeBlend * 2 + minimumBiomeSize * 2), biomeScale, 20, 0.05f, 20, 0.0045f, biomeRequirements);

        int previousBiomeID = biomeMap[0];
        int currentBiomeLength = 1;

        for(int t = 0; t < 3; t++)
        {
            for (int i = 1; i < biomeMap.Length; i++)
            {
                //Cleaning out biomes that are way too damn small.

                if (biomeMap[i] == previousBiomeID)
                {
                    currentBiomeLength += 1;
                }
                else
                {
                    if (currentBiomeLength < minimumBiomeSize)
                    {
                        for (int j = 1; j < currentBiomeLength + 1; j++)
                        {
                            if (i - j > 0) biomeMap[i - j] = biomeMap[i];
                        }
                    }
                    else
                    {
                        currentBiomeLength = 1;
                    }

                    previousBiomeID = biomeMap[i];

                }
            }
        }

        //Used for predictable random values from 0-1.
        float[,] staticNoiseMap = null;
        //Used for the swirly texture you see in between biomes to transition them smoothly
        float[,] biomeTransitionStaticMap = null;

        Coroutine staticNoiseMap_GEN = StartCoroutine(EfficientGPNP(width, height, random.Next(), new Octave[] { new Octave(0.789f, 1f, new Vector2(random.Next(1000000, 4000000), random.Next(1000000, 4000000))) }, 1, 1, new Vector2Int(globalOffset.x, -globalOffset.y - height), 
            (map) => {
                staticNoiseMap = map;
            }));
        Coroutine biomeTransitionStaticMap_GEN = StartCoroutine(EfficientGPNP(width, height, random.Next(), new Octave[] { new Octave(0.06f, 0.78f, new Vector2(random.Next(1000000, 4000000), random.Next(1000000, 4000000))), new Octave(0.212f, 0.27f, Vector2.zero) }, 1, 1, new Vector2Int(globalOffset.x, -globalOffset.y - height),
            (map) => {
                biomeTransitionStaticMap = map;
            }));

        yield return staticNoiseMap_GEN;
        yield return biomeTransitionStaticMap_GEN;

        generatedBiomeMaps = 0;
        requiredBiomeMaps = 0;

        for(int b = 0; b < biomes.Length; b++)
        {
            //Generating the hills of each biome (does not generate biome sheets that do not exist in the specified width)

            bool biomeMapHasBiome = false;

            for (int i = 0; i < biomeMap.Length; i++)
            {
                if (biomeMap[i] == biomes[b].biomeID)
                {
                    requiredBiomeMaps++;
                    biomeMapHasBiome = true;
                    break;
                }
            }

            Biome targetBiome = biomes[b];

            if (biomeMapHasBiome)
            {

                StartCoroutine(EfficientGPNP(width, 1, random.Next(), biomes[b].biomeOctaves, 1, 1, new Vector2Int(-biomes[b].biomeOffset, 0) + new Vector2Int(globalOffset.x, 0),
                    (map) =>
                    {

                        targetBiome.surfaceMap = map;
                        generatedBiomeMaps += 1;

                    }));

            }
            else
            {
                random.Next();
            }

            //Reset the positivePositions, min/maxes, and seed sibling bool of every biome sheet to null

            for (int s = 0; s < biomes[b].sheets.Length; s++)
            {
                biomes[b].sheets[s].positivePositions = null;
                biomes[b].sheets[s].minY = int.MaxValue;
                biomes[b].sheets[s].maxY = int.MinValue;
                biomes[b].sheets[s].preMadeSeed = -1;
                biomes[b].sheets[s].noiseMap = null;
            }
        }

        yield return new WaitUntil(() => requiredBiomeMaps == generatedBiomeMaps);

        //Reset the positivePositions, min/maxes, and seed sibling bool of every global sheet to null

        for (int gs = 0; gs < globalSheets.Length; gs++)
        {
            globalSheets[gs].positivePositions = null;
            globalSheets[gs].minY = int.MaxValue;
            globalSheets[gs].maxY = int.MinValue;
            globalSheets[gs].preMadeSeed = -1;
            globalSheets[gs].noiseMap = null;
        }

        //Main x/y Iteration

        int noiseMapsToGenerate = 0;

        for (int x = 0; x < width; x++)
        {
            //Biome blending setup

            int centerBiomeID = biomeMap[x + biomeBlend * 2 + minimumBiomeSize * 2];
            int leftBiomeID = -1;
            int rightBiomeID = -1;
            int distanceToLeftBiome = int.MaxValue;
            int distanceToRightBiome = int.MaxValue;

            for(int i = 1; i < biomeBlend + 1; i++)
            {
                if (rightBiomeID != -1 && leftBiomeID != -1) break;

                if(rightBiomeID == -1)
                {
                    int right_i_BiomeIDValue = biomeMap[x + biomeBlend * 2 + minimumBiomeSize * 2 + i];

                    if (right_i_BiomeIDValue != centerBiomeID)
                    {
                        rightBiomeID = right_i_BiomeIDValue;
                        distanceToRightBiome = i;
                    }
                }

                if(leftBiomeID == -1)
                {
                    int left_i_BiomeIDValue = biomeMap[x + biomeBlend * 2 + minimumBiomeSize * 2 - i];

                    if (left_i_BiomeIDValue != centerBiomeID)
                    {
                        leftBiomeID = left_i_BiomeIDValue;
                        distanceToLeftBiome = i;
                    }
                }
            }

            int centerBiomeIndex = -1;
            int leftBiomeIndex = -1;
            int rightBiomeIndex = -1;

            for (int b = 0; b < biomes.Length; b++)
            {
                if (centerBiomeID == biomes[b].biomeID)
                {
                    centerBiomeIndex = b;
                }
                else if(leftBiomeID != -1 && leftBiomeID == biomes[b].biomeID)
                {
                    leftBiomeIndex = b;
                }
                else if (rightBiomeID != -1 && rightBiomeID == biomes[b].biomeID)
                {
                    rightBiomeIndex = b;
                }
            }

            
            //Blending biome heights

            float rawTopLandLevel = 0;

            if(centerBiomeIndex != -1) rawTopLandLevel = height - biomes[centerBiomeIndex].surfaceMap[x, 0] + biomes[centerBiomeIndex].biomeHeight;

            if (leftBiomeIndex != -1)
            {
                rawTopLandLevel = Mathf.Lerp((height - biomes[leftBiomeIndex].surfaceMap[x, 0] + biomes[leftBiomeIndex].biomeHeight), rawTopLandLevel, biomeBlendCurve.Evaluate(0.5f + ((float)distanceToLeftBiome / (float)biomeBlend * 0.5f)));
            }

            if (rightBiomeIndex != -1)
            {
                rawTopLandLevel = Mathf.Lerp(rawTopLandLevel, (height - biomes[rightBiomeIndex].surfaceMap[x, 0] + biomes[rightBiomeIndex].biomeHeight), biomeBlendCurve.Evaluate((1 - (float)distanceToRightBiome / (float)biomeBlend) * 0.5f));
            }

            int topLandLevel = Mathf.FloorToInt(rawTopLandLevel) + globalOffset.y;

            for (int y = 0; y < height; y++)
            {
                //Blending biome values

                int XYBiomeIndex;
                int chosenBiomeID = -1;

                if (leftBiomeIndex != -1 && rightBiomeIndex == -1)
                {
                    XYBiomeIndex = biomeTransitionStaticMap[x, y] <= 0.5f * Mathf.Clamp01(1 - ((distanceToLeftBiome - biomeBlend + biomeBlendFalloff) / biomeBlendFalloff)) ? leftBiomeIndex : centerBiomeIndex;

                }
                else if (rightBiomeIndex != -1 && leftBiomeIndex == -1)
                {
                    XYBiomeIndex = biomeTransitionStaticMap[x, y] * Mathf.Clamp01(1 - ((distanceToRightBiome - biomeBlend + biomeBlendFalloff) / biomeBlendFalloff)) <= 0.5f ? centerBiomeIndex : rightBiomeIndex;
                }
                else
                {
                    XYBiomeIndex = centerBiomeIndex;
                }

                if (XYBiomeIndex == centerBiomeIndex) chosenBiomeID = centerBiomeID;
                else if (XYBiomeIndex == rightBiomeIndex) chosenBiomeID = rightBiomeID;
                else if (XYBiomeIndex == leftBiomeIndex) chosenBiomeID = leftBiomeID;

                if (XYBiomeIndex == -1)
                {
                    continue;
                }

                float extraHeight = globalOffset.y + biomes[XYBiomeIndex].biomeHeight + height;

                if(y < bottomLevel + globalOffset.y + height)
                {

                }
                else if (y > topLandLevel)
                {
                    //Generate water
                    if (y <= waterLevel + globalOffset.y + height && !(onlyValue.HasValue && onlyValue.Value != biomes[XYBiomeIndex].waterValue)) finalMap[x,y] = biomes[XYBiomeIndex].waterValue;
                }
                else
                {
                    //Set pixel to main value of biome. this can be overrided with sheets

                    if(!(onlyValue.HasValue && onlyValue.Value != biomes[XYBiomeIndex].mainValue)) finalMap[x, y] = biomes[XYBiomeIndex].mainValue;

                    //Change that pixel based on its biome's sheets + global sheets

                    for (int s = 0; s < globalSheets.Length + biomes[XYBiomeIndex].sheets.Length; s++)
                    {
                        Sheet targetSheet;

                        if (s >= globalSheets.Length)
                        {
                            targetSheet = biomes[XYBiomeIndex].sheets[s - globalSheets.Length];
                        }
                        else
                        {
                            targetSheet = globalSheets[s];
                        }

                        if (!targetSheet.use || (onlyValue.HasValue && onlyValue.Value != targetSheet.value)) continue;

                        float sheetRangeMin = targetSheet.range.x;
                        float sheetRangeMax = targetSheet.range.y;
                        float constantFeatherMin = targetSheet.constantFeatherRange.x;
                        float constantFeatherMax = targetSheet.constantFeatherRange.y;
                        float sheetFeather = targetSheet.feather;
                        float sheetFeatherSlope = targetSheet.featherSlope;
                        FadeType fadeType = targetSheet.fadeType;

                        bool pixelIsInYRange = false;
                        float featherDelta = 1;

                        if(targetSheet.featherType == FeatherType.Variable)
                        {
                            //Detecting whether the pixel is valid in a Variable fashion.

                            if (topLandLevel - y <= -sheetRangeMin + sheetFeather && topLandLevel - y >= -sheetRangeMax - sheetFeather)
                            {
                                if (topLandLevel - y <= -sheetRangeMin + sheetFeather && topLandLevel - y > -sheetRangeMin)
                                {
                                    featherDelta = 1 - (topLandLevel - y + sheetRangeMin) * sheetFeatherSlope / sheetFeather;

                                    if (fadeType == FadeType.Noisy)
                                    {
                                        float randomPercentSheetLower = staticNoiseMap[x, y] / 0.75f;

                                        if (randomPercentSheetLower <= featherDelta)
                                        {
                                            pixelIsInYRange = true;
                                        }
                                    }
                                    else
                                    {
                                        pixelIsInYRange = true;
                                    }
                                }
                                else if (topLandLevel - y >= -sheetRangeMax - sheetFeather && topLandLevel - y < -sheetRangeMax)
                                {
                                    featherDelta = 1 + (topLandLevel - y + sheetRangeMax) * sheetFeatherSlope / sheetFeather;

                                    if (fadeType == FadeType.Noisy)
                                    {
                                        float randomPercentSheetUpper = staticNoiseMap[x, y] / 0.75f;

                                        if (randomPercentSheetUpper <= featherDelta)
                                        {
                                            pixelIsInYRange = true;
                                        }
                                    }
                                    else
                                    {
                                        pixelIsInYRange = true;
                                    }
                                }
                                else
                                {
                                    pixelIsInYRange = true;
                                }
                            }
                        }
                        else
                        {
                            //Detecting whether the pixel is valid in a Constant fashion.

                            if (topLandLevel - y <= -sheetRangeMin && topLandLevel - y >= -sheetRangeMax)
                            {
                                if (y <= constantFeatherMin + extraHeight)
                                {
                                    featherDelta = 1 - ((constantFeatherMin + extraHeight) - y) * sheetFeatherSlope / sheetFeather;

                                    if (fadeType == FadeType.Noisy)
                                    {
                                        float randomPercentSheetLower = staticNoiseMap[x, y] / 0.65f;

                                        if (randomPercentSheetLower <= featherDelta)
                                        {
                                            pixelIsInYRange = true;
                                        }
                                    }
                                    else
                                    {
                                        pixelIsInYRange = true;
                                    }
                                }
                                else if (y >= constantFeatherMax + extraHeight)
                                {
                                    featherDelta = 1 - (y - (constantFeatherMax + extraHeight)) * sheetFeatherSlope / sheetFeather;

                                    if (fadeType == FadeType.Noisy)
                                    {
                                        float randomPercentSheetUpper = staticNoiseMap[x, y] / 0.65f;

                                        if (randomPercentSheetUpper <= featherDelta)
                                        {
                                            pixelIsInYRange = true;
                                        }
                                    }
                                    else
                                    {
                                        pixelIsInYRange = true;
                                    }
                                }
                                else
                                {
                                    pixelIsInYRange = true;
                                }
                            }
                        }

                        if (pixelIsInYRange)
                        {
                            //Do this is the y level is in a valid place

                            int targetValue = targetSheet.value;

                            if (targetSheet is GlobalSheet && ((GlobalSheet)targetSheet).biomeValuePairings.Length > 0)
                            {
                                //If this sheet is a global sheet, make sure the targetValue is set based on the biome if there are biomeValuePairings relavent to the biome of this pixel

                                GlobalSheet globalSheet = (GlobalSheet)targetSheet;
                                for (int i = 0; i < globalSheet.biomeValuePairings.Length; i++)
                                {
                                    if (globalSheet.biomeValuePairings[i].biomeID == chosenBiomeID)
                                    {
                                        targetValue = globalSheet.biomeValuePairings[i].pairedValue;
                                        break;
                                    }
                                }
                            }

                            //Now we know that pixels can appear here (the sheet is not out of scope of the generation window), we can can add this position to the respective HashSet of globalSheetThatNeedMaps.

                            if (targetSheet.positivePositions == null)
                            {
                                targetSheet.positivePositions = new PositivePosition[width, height];
                                if(targetSheet.sheetOctaves.Length != 0) noiseMapsToGenerate += 1;

                            }

                            if (y < targetSheet.minY) targetSheet.minY = y;
                            if (y > targetSheet.maxY) targetSheet.maxY = y;
                            targetSheet.positivePositions[x, y] = new PositivePosition(featherDelta, targetValue);
                        }
                    }
                }
            }
        }

        //Generate each noisemap of each sheet that needs to be generated. This is multithreaded.

        int detectedNoiseMapsGenerated = 0;

        for(int b = 0; b < biomes.Length; b++)
        {
            for (int s = 0; s < globalSheets.Length + biomes[b].sheets.Length; s++)
            {
                Sheet targetSheet;
                Sheet nextSheet = null;

                if (s < globalSheets.Length)
                {
                    targetSheet = globalSheets[s];
                    if (s + 1 < globalSheets.Length) nextSheet = globalSheets[s + 1];

                }
                else
                {
                    targetSheet = biomes[b].sheets[s - globalSheets.Length];
                    if (s + 1 < globalSheets.Length + biomes[b].sheets.Length) nextSheet = biomes[b].sheets[s + 1 - globalSheets.Length];
                }

                int nextSheetRandom = sheetRandom.Next();

                if (!targetSheet.use || (onlyValue.HasValue && onlyValue.Value != targetSheet.value) || targetSheet.noiseMap != null) continue;

                if (nextSheet != null && nextSheet.usePreviousSheetSeed)
                {
                    if(targetSheet.preMadeSeed != -1)
                    {
                        nextSheet.preMadeSeed = targetSheet.preMadeSeed;
                    }
                    else
                    {
                        nextSheet.preMadeSeed = nextSheetRandom;
                    }
                }

                if (targetSheet.sheetOctaves.Length != 0 && targetSheet.positivePositions != null)
                {

                    targetSheet.noiseMap = new float[0, 0];

                    StartCoroutine(EfficientGPNP(width, targetSheet.maxY - targetSheet.minY + 1, targetSheet.preMadeSeed != -1 ? targetSheet.preMadeSeed : nextSheetRandom, targetSheet.sheetOctaves, 1, 1, targetSheet.sheetOffset + new Vector2Int(globalOffset.x, -globalOffset.y - height + targetSheet.minY),
            (map) => {

                            targetSheet.noiseMap = map;
                            detectedNoiseMapsGenerated += 1;

                            }));
                }
            }
        }

        //Making sheet maps...
        yield return new WaitUntil(() => noiseMapsToGenerate == detectedNoiseMapsGenerated);

        string sheetExecutionOrder = "";

        for (int b = 0; b < biomes.Length; b++)
        {
            bool hasLocalSheetsPositionIndicator = false;
            int lastGlobalIndex = -1;
            int localSheetsLeftToIterateThrough = 0;

            for (int s = 0; s < globalSheets.Length + biomes[b].sheets.Length; s++)
            {
                //Set target sheet based on order of execution (consider the LOCAL_SHEETS separator if it exists)

                Sheet targetSheet;

                if (s >= globalSheets.Length && !hasLocalSheetsPositionIndicator)
                {
                    targetSheet = biomes[b].sheets[s - globalSheets.Length];

                }
                else if (localSheetsLeftToIterateThrough != 0)
                {
                    targetSheet = biomes[b].sheets[biomes[b].sheets.Length - localSheetsLeftToIterateThrough];
                    
                    localSheetsLeftToIterateThrough -= 1;

                    
                }
                else
                {
                    lastGlobalIndex += 1;
                    targetSheet = globalSheets[lastGlobalIndex];

                    if (targetSheet.name == "LOCAL_SHEETS")
                    {
                        hasLocalSheetsPositionIndicator = true;
                        localSheetsLeftToIterateThrough = biomes[b].sheets.Length;
                        continue;
                    }
                }

                //Now that we have the sheet based on its order of execution, let's start going through positive positions.

                if (targetSheet.positivePositions != null)
                {
                    sheetExecutionOrder += targetSheet.name + ",";

                    for (int x = 0; x < targetSheet.positivePositions.GetLength(0); x++)
                    {
                        for (int y = 0; y < targetSheet.positivePositions.GetLength(1); y++)
                        {
                            if (targetSheet.positivePositions[x, y] != null)
                            {
                                if(targetSheet.noiseMap == null)
                                {
                                    finalMap[x, y] = targetSheet.positivePositions[x, y].positivePixelValue;
                                }
                                else
                                {
                                    float areaNoiseCalc = targetSheet.noiseMap[x, y - targetSheet.minY];

                                    if (targetSheet.fadeType == FadeType.OctaveAmplitude)
                                    {
                                        areaNoiseCalc *= targetSheet.positivePositions[x, y].featherDelta;
                                    }

                                    if (areaNoiseCalc >= 0.5f && areaNoiseCalc <= 1 - targetSheet.damplitude)
                                    {
                                        finalMap[x, y] = targetSheet.positivePositions[x, y].positivePixelValue;
                                    }
                                    else if (targetSheet.onlyPositivePerlins)
                                    {
                                        int finalValue = 0;

                                        //Let the liquid values fill in blank spaces created by an onlyPositivePerlin generation

                                        for (int w = 0; w < liquidValues.Length; w++)
                                        {
                                            if (y < height - 1 && finalMap[x, y + 1] == liquidValues[w])
                                            {
                                                finalValue = liquidValues[w];
                                            }
                                            else if (x > 0 && finalMap[x - 1, y] == liquidValues[w])
                                            {
                                                finalValue = liquidValues[w];
                                            }
                                            else if (x < width - 1 && finalMap[x + 1, y] == liquidValues[w])
                                            {
                                                finalValue = liquidValues[w];
                                            }
                                        }

                                        finalMap[x, y] = finalValue;
                                    }
                                }
                            }
                        }
                    }

                    targetSheet.positivePositions = null;
                }
            }
        }

        //runtimeStopwatch.Stop();
        //Debug.Log($"Generated in {runtimeStopwatch.ElapsedMilliseconds}ms.");
        toDoWithMap(finalMap);
    }

    public static int[] GenerateBiomeMap(System.Random rand, int width, int offset, float globalBiomeScale, float hAmp, float hFreq, float tAmp, float tFreq, BiomeRequirement[] biomeRequirements)
    {
        int[] finalMap = new int[width];

        int heightRandomOffset = rand.Next(1000000, 4000000);
        int tempRandomOffset = rand.Next(1000000, 4000000);
        int heightRandomAlteration = rand.Next(1000000, 4000000);
        int tempRandomAlteration = rand.Next(1000000, 4000000);

        for (int x = 0; x < width; x++)
        {
            finalMap[x] = -1;

            float hHeight = Mathf.PerlinNoise((x + offset + heightRandomOffset) * hFreq * (1 / globalBiomeScale), heightRandomAlteration);
            float heightPass = hHeight * hAmp;

            float tHeight = Mathf.PerlinNoise((x + offset + tempRandomOffset) * tFreq * (1 / globalBiomeScale), tempRandomAlteration);
            float tempPass = tHeight * tAmp;

            foreach (BiomeRequirement br in biomeRequirements)
            {
                if (heightPass >= br.heightRange.x && heightPass <= br.heightRange.y)
                {
                    if (tempPass >= br.temperatureRange.x && tempPass <= br.temperatureRange.y)
                    {
                        finalMap[x] = br.forBiomeID;
                        break;
                    }
                }
            }

        }

        return finalMap;

    }

    public enum PerlinBehaviour { Regular, SmoothStart, InvertedSmoothStart }
    public struct EfficientGPNPJob : IJob
    {
        public int mapWidth;
        public int mapHeight;
        public int seed;
        public FirstFiveOctaves firstFiveOctaves;
        public float mapFrequency;
        public float mapAmplitude;
        public Vector2Int mapOffset;
        public NativeArray<float> result;

        public void Execute()
        {
            Vector2[] octaveExtraOffsets = new Vector2[0];

            if (seed != -1)
            {
                System.Random branchedRandom = new System.Random(seed);

                octaveExtraOffsets = new Vector2[firstFiveOctaves.count];

                for (int e = 0; e < octaveExtraOffsets.Length; e++)
                {
                    octaveExtraOffsets[e].x = branchedRandom.Next(1000000, 4000000);
                    octaveExtraOffsets[e].y = branchedRandom.Next(1000000, 4000000);
                }
            }

            int i = 0;

            for (int y = 0; y < mapHeight; y++)
            {
                for (int x = 0; x < mapWidth; x++)
                {
                    float noiseHeight = 0;

                    if (firstFiveOctaves.octave1.biomePerlinBehaviour == PerlinBehaviour.InvertedSmoothStart) noiseHeight = 1;

                    for (int o = 0; o < firstFiveOctaves.count; o++)
                    {
                        if (!firstFiveOctaves.GetOctave(o).use) continue;

                        float sampleX = ((x + firstFiveOctaves.GetOctave(o).offset.x + mapOffset.x + octaveExtraOffsets[o].x) * firstFiveOctaves.GetOctave(o).frequency * mapFrequency);
                        float sampleY = ((y + firstFiveOctaves.GetOctave(o).offset.y + mapOffset.y + octaveExtraOffsets[o].y) * firstFiveOctaves.GetOctave(o).frequency * mapFrequency);

                        float perlinValue = Mathf.PerlinNoise(sampleX, sampleY);

                        if (firstFiveOctaves.GetOctave(o).biomePerlinBehaviour == PerlinBehaviour.SmoothStart || firstFiveOctaves.GetOctave(o).biomePerlinBehaviour == PerlinBehaviour.InvertedSmoothStart) perlinValue = perlinValue * perlinValue * perlinValue * perlinValue * perlinValue * perlinValue;

                        noiseHeight += perlinValue * firstFiveOctaves.GetOctave(o).amplitude * mapAmplitude;

                    }

                    result[i] = noiseHeight;

                    i++;
                }
            }
        }
    }
    public IEnumerator EfficientGPNP(int mapWidth, int mapHeight, int seed, Octave[] octaves, float mapFrequency, float mapAmplitude, Vector2Int mapOffset, System.Action<float[,]> toDoWithMap)
    {
        NativeArray<float> result = new NativeArray<float>(mapWidth * mapHeight, Allocator.Persistent);
        EfficientGPNPJob job = new EfficientGPNPJob
        {
            mapWidth = mapWidth,
            mapHeight = mapHeight,
            seed = seed,
            firstFiveOctaves = new FirstFiveOctaves(octaves),
            mapFrequency = mapFrequency,
            mapAmplitude = mapAmplitude,
            mapOffset = mapOffset,
            result = result,
        };

        JobHandle jobHandle = job.Schedule();

        yield return new WaitUntil(() => jobHandle.IsCompleted);

        jobHandle.Complete();

        float[,] generated = new float[mapWidth, mapHeight];

        for (int y = 0; y < mapHeight; y++)
        {
            for (int x = 0; x < mapWidth; x++)
            {
                generated[x, y] = result[x + y * mapWidth];
            }
        }

        result.Dispose();
        toDoWithMap(generated);

    }

    [Serializable]
    public struct Octave
    {
        public bool use;
        public PerlinBehaviour biomePerlinBehaviour;
        [Range(0.0001f, 1f)]
        public float frequency;
        public float amplitude;
        public Vector2 offset;
        public Octave(float frequency, float amplitude, Vector2 offset)
        {
            this.biomePerlinBehaviour = PerlinBehaviour.Regular;
            this.use = true;
            this.frequency = frequency;
            this.amplitude = amplitude;
            this.offset = offset;
        }
    }

    public struct FirstFiveOctaves
    {
        public Octave octave1 { get; }
        public Octave octave2 { get; }
        public Octave octave3 { get; }
        public Octave octave4 { get; }
        public Octave octave5 { get; }

        public bool isEmpty { get; }
        public bool hasOctave2 { get; }
        public bool hasOctave3 { get; }
        public bool hasOctave4 { get; }
        public bool hasOctave5 { get; }

        public int count;

        public FirstFiveOctaves(Octave[] octaves)
        {
            count = Mathf.Min(octaves.Length, 5);

            if(octaves.Length >= 1)
            {
                this.octave1 = octaves[0];
                isEmpty = false;

                if (octaves.Length >= 2)
                {
                    this.octave2 = octaves[1];
                    hasOctave2 = true;

                    if (octaves.Length >= 3)
                    {
                        this.octave3 = octaves[2];
                        hasOctave3 = true;

                        if (octaves.Length >= 4)
                        {
                            this.octave4 = octaves[3];
                            hasOctave4 = true;

                            if (octaves.Length >= 5)
                            {
                                this.octave5 = octaves[4];
                                hasOctave5 = true;
                            }
                            else
                            {
                                this.octave5 = new Octave();
                                hasOctave5 = false;
                            }

                        }
                        else
                        {
                            this.octave4 = new Octave();
                            this.octave5 = new Octave();
                            hasOctave4 = false;
                            hasOctave5 = false;
                        }

                    }
                    else
                    {
                        this.octave3 = new Octave();
                        this.octave4 = new Octave();
                        this.octave5 = new Octave();
                        hasOctave3 = false;
                        hasOctave4 = false;
                        hasOctave5 = false;
                    }
                }
                else
                {
                    this.octave2 = new Octave();
                    this.octave3 = new Octave();
                    this.octave4 = new Octave();
                    this.octave5 = new Octave();
                    hasOctave2 = false;
                    hasOctave3 = false;
                    hasOctave4 = false;
                    hasOctave5 = false;
                }
            }
            else
            {
                this.octave1 = new Octave();
                this.octave2 = new Octave();
                this.octave3 = new Octave();
                this.octave4 = new Octave();
                this.octave5 = new Octave();
                this.hasOctave2 = false;
                this.hasOctave3 = false;
                this.hasOctave4 = false;
                this.hasOctave5 = false;

                isEmpty = true;
            }
        }

        public Octave GetOctave(int index)
        {
            if (index == 0 && !this.isEmpty) return octave1;
            if (index == 1 && this.hasOctave2) return octave2;
            if (index == 2 && this.hasOctave3) return octave3;
            if (index == 3 && this.hasOctave4) return octave4;
            if (index == 4 && this.hasOctave5) return octave5;

            Debug.Log("Fetched octave outside of range!");
            return new Octave();
        }
    }

    [Serializable]
    public class Biome
    {
        public string name = "New Biome";
        public int biomeID;
        public int mainValue;
        public int waterValue;

        public Octave[] biomeOctaves;
        public int startSheetVarianceAtOctave;
        public Sheet[] sheets;

        public int biomeHeight = 0;
        public int biomeOffset;

        [HideInInspector]
        public float[,] surfaceMap;
    }

    [Serializable]
    public class Sheet
    {
        public string name = "New Sheet";
        public bool use = true;
        public int value;
        //Any pixels that do not match the generation's octaves in the sheet's range will be forced to 0.
        public bool onlyPositivePerlins;
        public Vector2Int range = new Vector2Int(0, 0);
        public float feather = 0;
        public float featherSlope = 1;
        
        public enum FeatherType { Variable, Constant }
        //Variable = feathers based on the y trend that matches the biome's surface.
        //Constant = feathers based on concrete y values;
        public FeatherType featherType = FeatherType.Variable;

        public Vector2Int constantFeatherRange;

        public enum FadeType { Noisy, OctaveAmplitude}
        //Noisy = feather zone will have less pixels as it gets further away from the main zone
        //OctaveAmplitude = each octave's amplitude will approach 0 as it reaches the end of the feather. This will only work if you have octaves in your sheet.
        public FadeType fadeType = FadeType.Noisy;

        [Header("(Leave blank if you want sheet to generate solid)")]
        public Octave[] sheetOctaves;
        public float damplitude;
        public bool usePreviousSheetSeed;
        public Vector2Int sheetOffset;

        //Internal

        [HideInInspector]
        public PositivePosition[,] positivePositions = null;
        [HideInInspector]
        public int minY = int.MaxValue;
        [HideInInspector]
        public int maxY = int.MinValue;
        [HideInInspector]
        public int preMadeSeed = -1;
        [HideInInspector]
        public float[,] noiseMap;
    }

    public class PositivePosition
    {
        public float featherDelta = 1;
        public int positivePixelValue;

        public PositivePosition(float featherDelta, int positivePixelValue)
        {
            this.featherDelta = featherDelta;
            this.positivePixelValue = positivePixelValue;
        }
    }

    [Serializable]
    public class GlobalSheet : Sheet
    {
        [Header("Global Sheet Settings")]
        public BiomeValuePairing[] biomeValuePairings;
    }

    [Serializable]
    public class BiomeValuePairing
    {
        public int biomeID;
        public int pairedValue;
    }

    [Serializable]
    public class BiomeRequirement
    {
        public string label;
        public int forBiomeID;
        public Vector2 heightRange;
        public Vector2 temperatureRange;
        public bool isBeach;
    }
}
