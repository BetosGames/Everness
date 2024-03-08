using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Data;
using UnityEngine;
using UnityEngine.Tilemaps;
using static Extra;

public class Registry : MonoBehaviour
{
    //Singleton
    public static Registry INSTANCE = null;

    public static Dictionary<string, Tile> tileRegistry = new Dictionary<string, Tile>();
    private static Dictionary<int, Tile> coreIDRegistry = new Dictionary<int, Tile>();
    private static Dictionary<string, Item> itemRegistry = new Dictionary<string, Item>();
    private static Dictionary<string, Command> commandRegistry = new Dictionary<string, Command>();
    public EntityEntry[] entityRegistry;
    

    private void Awake()
    {
        INSTANCE = this;

        RegisterTiles();
        RegisterItems();
        RegisterCommands();
    }

    private void RegisterTiles()
    {
        //==============Register new tiles here!!==============
        RegisterTile(new TileAirBack());
        RegisterTile(new TileAirMain());
        RegisterTile(new TileAirFore());
        RegisterTile(new TileAdobeCave());
        RegisterTile(new TileBlackGraniteCave());
        RegisterTile(new TileDirtCave());
        RegisterTile(new TileFrostsandCave());
        RegisterTile(new TileHardpanCave());
        RegisterTile(new TileMantleCave());
        RegisterTile(new TileRuckCave());
        RegisterTile(new TileSandCave());
        RegisterTile(new TileSnowCave());
        RegisterTile(new TileStoneCave());
        RegisterTile(new TileAdobe());
        RegisterTile(new TileAluminumOre());
        RegisterTile(new TileAmethystOre());
        RegisterTile(new TileAncientBrick());
        RegisterTile(new TileBauxite());
        RegisterTile(new TileBlackGranite());
        RegisterTile(new TileBlackQuartzOre());
        RegisterTile(new TileChromiumOre());
        RegisterTile(new TileClay());
        RegisterTile(new TileCopperOre());
        RegisterTile(new TileDeadGrassyDirt());
        RegisterTile(new TileDiamondOre());
        RegisterTile(new TileDirt());
        RegisterTile(new TileDirtCadmiumOre());
        RegisterTile(new TileDirtCitrineOre());
        RegisterTile(new TileDirtKyaniteOre());
        RegisterTile(new TileDirtOsmiumOre());
        RegisterTile(new TileDirtSulphurOre());
        RegisterTile(new TileDirtZirconOre());
        RegisterTile(new TileDryGrassyDirt());
        RegisterTile(new TileDryIce());
        RegisterTile(new TileEmeraldOre());
        RegisterTile(new TileFeldsparOre());
        RegisterTile(new TileFoolsGold());
        RegisterTile(new TileForestLeaves());
        RegisterTile(new TileFrostsand());
        RegisterTile(new TileGoldOre());
        RegisterTile(new TileGraphite());
        RegisterTile(new TileGrassyDirt());
        RegisterTile(new TileGravel());
        RegisterTile(new TileHardpan());
        RegisterTile(new TileIce());
        RegisterTile(new TileIcySnow());
        RegisterTile(new TileIronOre());
        RegisterTile(new TileJasperOre());
        RegisterTile(new TileLeadOre());
        RegisterTile(new TileLimestone());
        RegisterTile(new TileLoam());
        RegisterTile(new TileMagnesiumOre());
        RegisterTile(new TileMantle());
        RegisterTile(new TileMulch());
        RegisterTile(new TileNeonOre());
        RegisterTile(new TileNickelOre());
        RegisterTile(new TilePermafrost());
        RegisterTile(new TilePlutoniumOre());
        RegisterTile(new TilePotashOre());
        RegisterTile(new TilePumice());
        RegisterTile(new TileRubyOre());
        RegisterTile(new TileRuck());
        RegisterTile(new TileSand());
        RegisterTile(new TileSandCitrineOre());
        RegisterTile(new TileSandSulphurOre());
        RegisterTile(new TileSandZirconOre());
        RegisterTile(new TileSapphireOre());
        RegisterTile(new TileSiliconOre());
        RegisterTile(new TileSlate());
        RegisterTile(new TileSnow());
        RegisterTile(new TileSnowCadmiumOre());
        RegisterTile(new TileSnowKyaniteOre());
        RegisterTile(new TileSnowOsmiumOre());
        RegisterTile(new TileSnowyDirt());
        RegisterTile(new TileStone());
        RegisterTile(new TileTalc());
        RegisterTile(new TileThoriumOre());
        RegisterTile(new TileTinOre());
        RegisterTile(new TileTitaniumOre());
        RegisterTile(new TileUraniumOre());
        RegisterTile(new TileZincOre());
        RegisterTile(new TileWalnutLog());
        RegisterTile(new TileWalnutLeaves());
        RegisterTile(new TileTallgrass());
        RegisterTile(new TileShortgrass());
        RegisterTile(new TileStrawberryBush());
        RegisterTile(new TileStickerBushTop());
        RegisterTile(new TileStickerBushBottom());
        RegisterTile(new TileTorch());

    }

    private static void RegisterItems()
    {
        //==============Register new items here!!==============
        RegisterItem(new ItemCopper());
    }

    private void RegisterCommands()
    {
        //==============Register new commands here!!==============
        RegisterCommand(new CommandSpawn());
        RegisterCommand(new CommandTeleport());
        RegisterCommand(new CommandGive());
        RegisterCommand(new CommandSpeed());
        RegisterCommand(new CommandJump());
    }

    private void RegisterTile(Tile tileReference)
    {
        tileRegistry.Add(tileReference.tileID, tileReference);
        if(tileReference.coreID > 0) coreIDRegistry.Add(tileReference.coreID, tileReference);
    }
    private static void RegisterItem(Item itemReference)
    {
        itemRegistry.Add(itemReference.itemID, itemReference);
    }
    private void RegisterCommand(Command command)
    {
        commandRegistry.Add(command.commandID, command);
    }

    public static Item NewItemFromID(string itemID)
    {
        return itemRegistry.ContainsKey(itemID) ? itemRegistry[itemID].copy() : null;
    }

    public static Tile NewTileFromID(string tileID)
    {
        return tileRegistry.ContainsKey(tileID) ? tileRegistry[tileID].copy() : null;
    }

    public static Tile NewTileFromCoreID(int coreID)
    {
        return coreIDRegistry.ContainsKey(coreID) ? coreIDRegistry[coreID].copy() : null;
    }

    public Command GetCommandFromID(string commandID)
    {
        return commandRegistry.ContainsKey(commandID) ? commandRegistry[commandID] : null;
    }

    public Entity GetEntityTypeFromID(string entityID)
    {
        foreach(EntityEntry entry in entityRegistry)
        {
            if(entry.ID == entityID)
            {
                return entry.prefab.GetComponent<Entity>();
            }
        }

        return null;
    }

    public GameObject GetPrefabFromEntityID(string entityID)
    {
        foreach (EntityEntry entry in entityRegistry)
        {
            if (entry.ID == entityID)
            {
                return entry.prefab;
            }
        }

        return null;
    }

    [System.Serializable]
    public class EntityEntry
    {
        public string ID;
        public GameObject prefab;
    }

    private static Dictionary<string, Texture2D> registeredTextures = new Dictionary<string, Texture2D>();
    private static Dictionary<string, Sprite> registeredSprites = new Dictionary<string, Sprite>();
    private static Dictionary<string, TileBase> registeredTileBases = new Dictionary<string, TileBase>();

    public static Texture2D GetTexture(string type, string id)
    {
        if(!registeredTextures.ContainsKey(type + ":" + id))
        {
            Texture2D newTexture = FileManager.TextureFromPNG($"{type}/{id}.png");
            registeredTextures.Add(type + ":" + id, newTexture);
        }

        return registeredTextures[type + ":" + id];
    }

    public static Sprite GetSprite(string type, string id)
    {
        if (!registeredSprites.ContainsKey(type + ":" + id))
        {
            Texture2D texture = GetTexture(type, id);
            Sprite newSprite = Sprite.Create(GetTexture(type, id), new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f), texture.width);
            registeredSprites.Add(type + ":" + id, newSprite);
        }

        return registeredSprites[type + ":" + id];
    }

    public static TileBase GetTileBase(string tileID)
    {
        if (!registeredTileBases.ContainsKey(tileID))
        {
            CustomTileBase customTileBase = (CustomTileBase)ScriptableObject.CreateInstance(typeof(CustomTileBase));
            customTileBase.sprite = Registry.GetSprite("tile", tileID);
            Tile tile = Registry.NewTileFromID(tileID);
            customTileBase.collidable = tile is TileMain && ((TileMain)tile).isCollidable;
            registeredTileBases.Add(tileID, customTileBase);
        }

        return registeredTileBases[tileID];
    }
}
