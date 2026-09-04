# Skeleton Revenge (Working Title)

**Skeleton Revenge** is a retro-style, 2.5D first-person shooter heavily inspired by the gritty aesthetic of the classic 1997 game *Blood*

## ⚙️ Technology
This project is built from scratch using a custom raycasting engine based on the [Lodev Raycasting Tutorial](https://lodev.org/cgtutor/raycasting.html). While it aims to capture the fast-paced feel of 90s shooters like *Blood* or *Doom*, it is built on pure raycasting mechanics rather than replicating the complex sector-based Build Engine.

To make my life easier I decided to use C# with MonoGame, so I don't have to care about all low-level stuff and can focus on engine and game itself.

## 🚀 Development Roadmap
The engine is currently in active development. Here is the checklist of planned features:

**Engine & Rendering:**
- [x] Drawing walls (with colors)
- [x] Wall Textures
- [x] Floor Casting
- [x] Ceiling Casting
- [x] 2D Sprite Rendering (Entities & Objects)
- [x] Load levels from XML
- [x] Weapon rendering (player holding weapon, or sword or something else...)
- [ ] Make level load player positon and direction from XML
- [ ] Distance fog
- [ ] Night sky with stars that rotate with players rotation
- [ ] Add collision to certain sprites
- [ ] More complex lighting system
- [ ] Rotated sprites, when entity is moving to left it should be facing left
- [ ] Animated sprites
- [ ] Make more weapons. Weapon class needs to be abstract (rename to BaseWeapon)
- [ ] Maybe create my own level editor???

**Gameplay & Content**
- [ ] Working Weapon System
- [ ] Decals
- [ ] View bobbing
- [ ] HUD
- [ ] Enemy AI and Combat
- [ ] More levels
- [ ] Story

**FIX THIS**
- [ ] Do I need to load every single texture in TextureManager?

## 📷 Pictures from each update of game
<table>
  <tr>
    <td align="center">
      <img src="https://github.com/user-attachments/assets/2785e911-1a6a-4dc6-a24f-c4009c868f19" alt="Update 1" width="100%"/>
      <br /><em>Update 1: Drawing Walls</em>
    </td>
    <td align="center">
      <img src="https://github.com/user-attachments/assets/6bf06cb5-d1e9-4cd6-952a-a34ce8b2882f" alt="Update 2" width="100%"/>
      <br /><em>Update 2: Textures</em>
    </td>
  </tr>
  <tr>
    <td align="center">
      <img src="https://github.com/user-attachments/assets/cb3fa8f1-5269-4139-8117-8f3691880416" alt="Update 3" width="100%"/>
      <br /><em>Update 3: Floor Casting</em>
    </td>
    <td align="center">
      <img src="https://github.com/user-attachments/assets/7c877944-693e-4835-900b-a865077da0fd" alt="Update 4" width="100%"/>
      <br /><em>Update 4: Ceiling Casting</em>
    </td>
  </tr>
  <tr>
    <td align="center">
      <img src="https://github.com/user-attachments/assets/22bafcf1-1bd6-4010-8566-3c44f81111c2" alt="Update 4" width="100%"/>
      <br /><em>Update 4: Sprite Rendering</em>
    </td>
  </tr>
</table>

## 🎮 Controls
⬆️ - Move forward

⬇️ - Move back

⬅️ - Rotate left

➡️ - Rotate right

## ⚠️ Disclaimer
*Note: This is personal project created for learning and development purposes. Textures and visual assets are currently sourced from Monolith Productions game "Blood (1997) and are used strictly as non-commercial placeholders.*
