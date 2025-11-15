# Regime Shop  
**Version:** 1.0.0  
**Author:** GaryG / Regime Gaming  

Regime Shop is a custom-built Rust in-game shop system with a clean UI, fast performance, and full compatibility with Economics and ImageLibrary.  
Designed specifically for Regime Gaming servers, this shop features a modern UI layout, category icons, enlarged item previews, dynamic balance display, and one-click purchases.

---

## ⭐ Features

- Modern Rust UI with transparent background
- Regime logo embedded in the header
- Category buttons with automatic Rust item icons
- 6x3 item grid per category (18 items per page)
- Enlarged item icons for visual clarity
- Price display directly under each item
- Click-to-buy purchase system
- Fully integrates with **Economics**
- Optional support for **ImageLibrary**
- Clean structure & easy to modify

---

## 📦 Requirements

The following Oxide plugins must be installed:

- **Economics** (required)  
  https://umod.org/plugins/economics  
- **ImageLibrary** (optional, recommended)  
  https://umod.org/plugins/imagelibrary  

If ImageLibrary is not detected, the plugin will still work but some icons may not load from URLs.

---

## 🛒 How It Works

Players open the shop by typing:

/shop

yaml
Copy code

The UI includes:

- A Regime logo on the top-left  
- The player's current Economics balance  
- Categories on the left side  
- A 6×3 grid of items  
- Prices clearly displayed  
- Clicking an item instantly buys it  

All items and prices are fully editable in the `categoryItems` dictionary inside the `.cs` file.

---

## ⚙️ Item Quantity Rules

Quantities for ammo, medical, and components are automatically assigned:

- Rifle ammo → 128  
- Shotgun ammo → 64  
- Pistol ammo → 128  
- Medical items → 10–20  
- Components → 25 per item  
- Everything else → 1  

Modify behavior in the `GetAmount` method.

---

## 🖼️ Icon System

The plugin uses:

- Default Rust icon lookups (`ItemId = …`)  
- Fallback icons via URL (ImageLibrary)  
- SteamCDN icons for consistent loading  

You can add your own icons inside the `imageUrls` dictionary.

---

## 🧰 Installation

1. Upload `RegimeShop.cs` to:  
   ```oxide/plugins/```

2. Reload Oxide or restart the server

3. Ensure Economics is loaded

4. Type `/shop` to open the UI

---

## 🧾 Configuration

No configuration file is created — all items and prices are edited directly inside:

private Dictionary<string, List<(string shortname, int price)>> categoryItems

perl
Copy code

Categories are defined here:

private readonly (string name, string icon)[] categories

yaml
Copy code

Everything is simple to manage, no JSON files required.

---

## 🛠️ Customization

This plugin is ideal for:

- PVE/PVP Rust servers  
- 2x, 5x, 10x modded setups  
- Clan/community servers  
- Custom economy or reward systems  

You may freely modify:

- Colors  
- Fonts  
- UI layout  
- Item prices  
- Item quantities  

---

## 📝 Credits

- Original UI structure by Regime Gaming  
- Logo & branding: Regime  
- Rust / CUI APIs by Facepunch & Oxide  

---

## 📄 License

Free to use, modify, and republish.  
Regime Gaming Edition.
