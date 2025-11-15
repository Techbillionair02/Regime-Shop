using Oxide.Core;
using Oxide.Game.Rust.Cui;
using UnityEngine;
using System.Collections.Generic;
using Oxide.Core.Plugins;

namespace Oxide.Plugins
{
    [Info("Regime Shop", "GaryG", "1.0.0")]
    public class RegimeShop : RustPlugin
    {
        [PluginReference] private Plugin Economics;
        [PluginReference] private Plugin ImageLibrary;

        // ============================================
        // CONSTANT UI NAMES
        // ============================================
        private const string ShopOverlayName = "RegimeShopOverlay";
        private const string ShopGridName = "RegimeShopGrid";

        // ============================================
        // CATEGORY DEFINITIONS
        // ============================================
        private readonly (string name, string icon)[] categories =
        {
            ("Weapons", "rifle.ak"),
            ("Attachments", "weapon.mod.holosight"),
            ("Ammunition", "ammo.rifle"),
            ("Attire", "hoodie"),
            ("Medical", "syringe.medical"),
            ("Tools", "jackhammer"),
            ("Misc", "keycard_red"),
            ("Component", "sewingkit"),
            ("Traps", "autoturret"),
            ("Construction", "hammer")
        };

        // ============================================
// CATEGORY ITEMS (FIXED + CLEAN SHORTNAMES)
// ============================================
private Dictionary<string, List<(string shortname, int price)>> categoryItems =
    new Dictionary<string, List<(string shortname, int price)>>
{
    ["Weapons"] = new List<(string, int)>
    {
        ("shotgun.waterpipe", 4),
        ("smg.thompson", 4),
        ("pistol.eoka", 1),
        ("pistol.revolver", 1),
        ("pistol.python", 4),
        ("smg.mp5", 15),
        ("shotgun.pump", 4),
        ("pistol.semiauto", 2),
        ("rifle.l96", 55),
        ("lmg.m249", 140),
        ("rifle.lr300", 25),
        ("pistol.m92", 5),
        ("rifle.ak", 50)
    },

    ["Attachments"] = new List<(string, int)>
    {
        ("weapon.mod.holosight", 2),
        ("weapon.mod.lasersight", 2),
        ("weapon.mod.small.scope", 3),       // FIXED NAME
        ("weapon.mod.muzzleboost", 1),
        ("weapon.mod.muzzlebrake", 1),
        ("weapon.mod.8x.scope", 1),
        ("weapon.mod.silencer", 1)
    },

    ["Ammunition"] = new List<(string, int)>
    {
        ("ammo.shotgun", 3),
        ("ammo.shotgun.fire", 4),
        ("ammo.shotgun.slug", 3),
        ("ammo.grenadelauncher.he", 3),
        ("ammo.grenadelauncher.buckshot", 3),
        ("ammo.grenadelauncher.smoke", 3),
        ("ammo.rifle", 2),
        ("ammo.rifle.hv", 3),
        ("arrow.fire", 3),
        ("ammo.pistol", 2),
        ("ammo.pistol.hv", 2),
        ("ammo.pistol.fire", 2)
    },

    ["Attire"] = new List<(string, int)>
    {
        ("coffeecan.helmet", 2),
        ("roadsign.jacket", 2),
        ("roadsign.kilt", 2),
        ("roadsign.gloves", 2),
        ("hoodie", 2),
        ("shoes.boots", 2),          // FIXED NAME
        ("pants", 2),
        ("metal.facemask", 5),
        ("metal.plate.torso", 4),
        ("heavy.plate.helmet", 4),
        ("heavy.plate.jacket", 4),
        ("heavy.plate.pants", 4),
        ("riot.helmet", 2),
        ("diving.fins", 2),
        ("diving.mask", 2),
        ("diving.tank", 2),
        ("diving.wetsuit", 2),
        ("horse.armor.roadsign", 3),
        ("tactical.gloves", 2),
        ("hazmatsuit", 3)
    },

    ["Medical"] = new List<(string, int)>
    {
        ("syringe.medical", 4),
        ("largemedkit", 2),
        ("bandage", 1),
        ("can.beans", 2)
    },

    ["Tools"] = new List<(string, int)>
    {
        ("tool.binoculars", 1),
        ("flashlight.held", 1),
        ("chainsaw", 2),
        ("jackhammer", 2)
    },

    ["Misc"] = new List<(string, int)>
    {
        ("electric.generator.small", 750),     // FIXED
        ("keycard_blue", 15),
        ("keycard_green", 5),
        ("keycard_red", 25),
        ("coffin.storage", 2)
    },

    ["Component"] = new List<(string, int)>
    {
        ("sewingkit", 4),
        ("sheetmetal", 2),
        ("tarp", 4),
        ("fuse", 1),
        ("propanetank", 2),
        ("gears", 5),
        ("roadsigns", 2),
        ("rope", 2),
        ("riflebody", 4),
        ("semibody", 2)
    },

    ["Traps"] = new List<(string, int)>
    {
        ("samsite", 30),
        ("flameturret", 3),
        ("trap.landmine", 1),
        ("guntrap", 4)
    },

    ["Construction"] = new List<(string, int)>
    {
        ("wall.frame.garagedoor", 2),
        ("gates.external.high.stone", 3),
        ("gates.external.high.wood", 2),
        ("floor.ladder.hatch", 2),
        ("shutter.metal.embrasure.a", 2),
        ("shutter.metal.embrasure.b", 2),
        ("floor.triangle.ladder.hatch", 2),
        ("barricade.wood", 1),
        ("barricade.metal", 3),
        ("ladder.wooden.wall", 2)
    }
};
// ============================================
// IMAGE URL DICTIONARY (FOR IMAGELIB)
// ============================================
private Dictionary<string, string> imageUrls = new Dictionary<string, string>
{
    { "rifle.m249", "https://steamcdn-a.akamaihd.net/apps/rust/icons/m249.png" },
    { "weapon.mod.x8", "https://steamcdn-a.akamaihd.net/apps/rust/icons/scope8x.png" },
    { "weapon.mod.sight", "https://steamcdn-a.akamaihd.net/apps/rust/icons/holosight.png" },
    { "ammo.pistol.hv", "https://steamcdn-a.akamaihd.net/apps/rust/icons/pistolammo_hv.png" },
    { "attire.helmet.coffee", "https://steamcdn-a.akamaihd.net/apps/rust/icons/coffeecanhelmet.png" },
    { "attire.armor.roadsign.gloves", "https://steamcdn-a.akamaihd.net/apps/rust/icons/roadsign.gloves.png" },
    { "roadsigns", "https://steamcdn-a.akamaihd.net/apps/rust/icons/roadsigns.png" },
    { "samsite", "https://steamcdn-a.akamaihd.net/apps/rust/icons/samsite.png" },
    { "landmine", "https://steamcdn-a.akamaihd.net/apps/rust/icons/landmine.png" },
    { "flameturret", "https://steamcdn-a.akamaihd.net/apps/rust/icons/flameturret.png" },
    { "electric.generator.small", "https://steamcdn-a.akamaihd.net/apps/rust/icons/smallgenerator.png" },
    { "door.garage", "https://steamcdn-a.akamaihd.net/apps/rust/icons/garagedoor.png" }
};

        // ============================================
        // CHAT COMMANDS
        // ============================================

        [ChatCommand("shop")]
        private void CmdShop(BasePlayer player, string cmd, string[] args)
        {
            ShowMainShop(player);
        }

        // ============================================
// CATEGORY SELECT (UI BUTTON TRIGGERED)
// ============================================
[ConsoleCommand("shop.select")]
private void CmdSelect(ConsoleSystem.Arg arg)
{
    BasePlayer player = arg.Player();
    if (player == null || arg.Args == null || arg.Args.Length < 1)
        return;

    string category = arg.Args[0];

    if (!categoryItems.ContainsKey(category))
    {
        player.ChatMessage("Invalid shop category.");
        return;
    }

    // Reload main UI + draw the grid for the chosen category
    ShowMainShop(player);
    ShowCategory(player, category);
}

        // ============================================
        // BUY COMMAND
        // ============================================

        [ConsoleCommand("regime.buy")]
        private void CmdBuy(ConsoleSystem.Arg arg)
        {
            BasePlayer player = arg.Player();
            if (player == null || arg.Args.Length < 2) return;

            string shortname = arg.Args[0];
            int price = int.Parse(arg.Args[1]);

            double balance = Economics?.Call<double>("Balance", player.userID) ?? 0;

            if (balance < price)
            {
                player.ChatMessage("Not enough balance.");
                return;
            }

            Economics?.Call("Withdraw", player.userID, (double)price);

            int amount = GetAmount(shortname);
            var item = ItemManager.CreateByName(shortname, amount);
            if (item != null)
                player.GiveItem(item);
        }

        // ============================================
        // MAIN SHOP UI
        // ============================================

        private void ShowMainShop(BasePlayer player)
        {
            DestroyUI(player);

            var container = new CuiElementContainer();

            // Background
            container.Add(new CuiPanel
            {
                Image = { Color = "0.22 0.22 0.22 0.70" },
                RectTransform = { AnchorMin = "0.15 0.22", AnchorMax = "0.85 0.88" },
                CursorEnabled = true
            }, "Hud", ShopOverlayName);

         // TOP-LEFT LOGO (before "Regime Shop")
container.Add(new CuiElement
{
    Parent = ShopOverlayName,
    Components =
    {
        new CuiRawImageComponent
        {
            Url = "https://i.imgur.com/Cm3HCUq.png"
        },
        new CuiRectTransformComponent
        {
            AnchorMin = "0.015 0.93",
            AnchorMax = "0.055 0.985"
        }
    }
});

// Header text (shifted right to make room for logo)
container.Add(new CuiLabel
{
    Text =
    {
        Text = "<b>Regime Shop</b> — Earn $20/hr • Discord https://discord.gg/7JuSjMhuGQ",
        FontSize = 18,
        Align = TextAnchor.MiddleLeft,
        Color = "1 1 1 1"
    },
    RectTransform =
    {
        AnchorMin = "0.065 0.93",
        AnchorMax = "0.98 0.985"
    }
}, ShopOverlayName);

            // Balance
            double balance = Economics?.Call<double>("Balance", player.userID) ?? 0.0;

            container.Add(new CuiLabel
            {
                Text =
                {
                    Text = $"<b>Balance: ${balance:0.00}</b>",
                    FontSize = 16,
                    Align = TextAnchor.MiddleCenter,
                    Color = "1 1 1 1"
                },
                RectTransform = { AnchorMin = "0.30 0.10", AnchorMax = "0.46 0.14" }
            }, ShopOverlayName);

            // Close button
            container.Add(new CuiButton
            {
                Button = { Command = "regime.close", Color = "0 0 0 0" },
                RectTransform = { AnchorMin = "0.90 0.935", AnchorMax = "0.985 0.98" },
                Text =
                {
                    Text = "<b>X Close</b>",
                    FontSize = 18,
                    Align = TextAnchor.MiddleCenter,
                    Color = "1 1 1 1"
                }
            }, ShopOverlayName);

            CreateCategoryButtons(container);

            CuiHelper.AddUi(player, container);
        }

        // ============================================
// CATEGORY BUTTONS (LEFT SIDE)
// ============================================
private void CreateCategoryButtons(CuiElementContainer container)
{
    float startY = 0.87f;
    float height = 0.06f;
    float gap = 0.012f;

    for (int i = 0; i < categories.Length; i++)
    {
        float top = startY - i * (height + gap);
        float bottom = top - height;

        string categoryName = categories[i].name;
        string icon = categories[i].icon;

        // CATEGORY ICON
        container.Add(new CuiElement
        {
            Parent = ShopOverlayName,
            Components =
            {
                new CuiImageComponent
                {
                    ItemId = ItemManager.FindItemDefinition(icon)?.itemid ?? 0
                },
                new CuiRectTransformComponent
                {
                    AnchorMin = $"0.01 {bottom + 0.01f}",
                    AnchorMax = $"0.035 {top - 0.01f}"
                }
            }
        });

        // CATEGORY BUTTON (THIS WAS BROKEN BEFORE)
        container.Add(new CuiButton
        {
            Button =
            {
                Command = $"shop.select {categoryName}",
                Color = "0.15 0.15 0.15 0.45"
            },
            RectTransform =
            {
                AnchorMin = $"0.04 {bottom}",
                AnchorMax = $"0.34 {top}"
            },
            Text =
            {
                Text = $"<b>{categoryName}</b>",
                FontSize = 17,
                Align = TextAnchor.MiddleLeft,
                Color = "1 1 1 1"
            }
        }, ShopOverlayName);
    }
}

        // ============================================
// CATEGORY ITEM GRID — MOVED LEFT (SIZE SAME)
// ============================================
private void ShowCategory(BasePlayer player, string category)
{
    if (!categoryItems.ContainsKey(category)) return;

    // Destroy old grid
    CuiHelper.DestroyUi(player, ShopGridName);

    var container = new CuiElementContainer();

    // SHIFTED MORE LEFT
    container.Add(new CuiPanel
    {
        Image = { Color = "0 0 0 0" },
        RectTransform =
        {
            AnchorMin = "0.22 0.18",   // moved left (0.26 → 0.22)
            AnchorMax = "0.90 0.90"    // adjusted right equally
        }
    }, ShopOverlayName, ShopGridName);

    int col = 0, row = 0;
    const int maxCols = 6;
    const int maxRows = 3;

    // TILE SIZE (same as before)
    float cellWidth = 0.14f;
    float cellHeight = 0.26f;

    // MATCH LEFT SHIFT
    float gridLeft = 0.22f;  // was 0.26f
    float gridTop = 0.90f;

    foreach (var (shortname, price) in categoryItems[category])
    {
        if (row >= maxRows) break;

        float xMin = gridLeft + (col * cellWidth);
        float xMax = xMin + (cellWidth - 0.01f);

        float yMax = gridTop - (row * cellHeight);
        float yMin = yMax - (cellHeight - 0.02f);

        // BACK PANEL
        container.Add(new CuiPanel
        {
            Image = { Color = "0.10 0.10 0.10 0.40" },
            RectTransform =
            {
                AnchorMin = $"{xMin} {yMin}",
                AnchorMax = $"{xMax} {yMax}"
            }
        }, ShopGridName);

        // ICON — proportional but 40% larger
container.Add(new CuiElement
{
    Parent = ShopGridName,
    Components =
    {
        new CuiImageComponent
        {
            ItemId = ItemManager.FindItemDefinition(shortname)?.itemid ?? 0
        },
        new CuiRectTransformComponent
        {
            // ICON SIZE BOOST — increase by shrinking padding
            AnchorMin = $"{xMin + 0.02f} {yMin + 0.04f}",   
            AnchorMax = $"{xMax - 0.02f} {yMax - 0.03f}"
        }
    }
});

        // PRICE (INSIDE BOX)
        container.Add(new CuiLabel
        {
            Text =
            {
                Text = $"<b>${price}</b>",
                FontSize = 17,
                Align = TextAnchor.LowerCenter,
                Color = "1 1 1 1"
            },
            RectTransform =
            {
                AnchorMin = $"{xMin} {yMin}",
                AnchorMax = $"{xMax} {yMin + 0.10f}"
            }
        }, ShopGridName);

        // BUY BUTTON
        container.Add(new CuiButton
        {
            Button =
            {
                Command = $"regime.buy {shortname} {price}",
                Color = "0 0 0 0"
            },
            RectTransform =
            {
                AnchorMin = $"{xMin} {yMin}",
                AnchorMax = $"{xMax} {yMax}"
            },
            Text = { Text = "" }
        }, ShopGridName);

        col++;
        if (col >= maxCols)
        {
            col = 0;
            row++;
        }
    }

    CuiHelper.AddUi(player, container);
}



        // ============================================
        // QUANTITY RULES
        // ============================================
        private int GetAmount(string sn)
        {
            if (sn.StartsWith("ammo.shotgun")) return 64;
            if (sn.StartsWith("ammo.grenadelauncher")) return 2;
            if (sn.StartsWith("ammo.rifle")) return 128;
            if (sn == "arrow.fire") return 128;
            if (sn.StartsWith("ammo.pistol")) return 128;

            if (sn == "syringe.medical") return 20;
            if (sn == "largemedkit") return 10;
            if (sn == "bandage") return 15;
            if (sn == "can.beans") return 5;

            if (sn == "fuse") return 2;
            if (sn == "riflebody") return 4;
            if (sn == "semibody") return 4;

            if (categoryItems["Component"].Exists(i => i.shortname == sn))
                return 25;

            return 1;
        }

        // ============================================
        // CLOSE
        // ============================================

        [ConsoleCommand("regime.close")]
        private void CmdClose(ConsoleSystem.Arg arg)
        {
            BasePlayer player = arg.Player();
            if (player != null)
                DestroyUI(player);
        }

        // ============================================
        // DESTROY UI
        // ============================================

        private void DestroyUI(BasePlayer player)
        {
            SafeDestroy(player, ShopOverlayName);
            SafeDestroy(player, ShopGridName);
        }

        private void SafeDestroy(BasePlayer player, string name)
        {
            if (player != null)
                CuiHelper.DestroyUi(player, name);
        }

        // ============================================
// INIT + IMAGE LOADER
// ============================================
void Init() => PrintWarning("Regime Shop Loaded.");

void OnServerInitialized()
{
    if (ImageLibrary == null)
    {
        PrintWarning("ImageLibrary not found — icons may not load.");
        return;
    }

    
}

void Unload()
{
    foreach (var p in BasePlayer.activePlayerList)
        DestroyUI(p);
}
}
}