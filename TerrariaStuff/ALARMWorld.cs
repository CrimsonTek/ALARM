using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ALARM.Core.Cables;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace ALARM.TerrariaStuff
{
    /// <summary>
    /// This is the world responsible for all ALARM related functions, including initializing Loader classes and saving.
    /// </summary>
    public class ALARMWorld : ModWorld
    {
        ///
        public override void PostDrawTiles()
        {
            SpriteBatchCables();
        }

        private static void SpriteBatchCables()
        {
            SpriteBatch spriteBatch = Main.spriteBatch;
            spriteBatch.Begin();

            try
            {
                DrawCables(spriteBatch);
            }
            catch (Exception inner)
            {
                spriteBatch.End();
                throw inner;
            }

            spriteBatch.End();
        }

        private static void DrawCables(SpriteBatch spriteBatch)
        {
            Dictionary<(int, int), Cable> cableDict = CableManager.cables;
            Vector2 screenPosition = Main.screenPosition;
            for (int i = 0; i < Main.screenWidth; i++)
            {
                int x = (int)screenPosition.X + i;
                for (int j = 0; j < Main.screenHeight; j++)
                {
                    int y = (int)screenPosition.Y + j;

                    if (cableDict.TryGetValue((x, y), out Cable cable))
                    {
                        if (cable.PreDraw(spriteBatch))
                        {
                            CableEntry entry = CableLoader.cableEntries[cable.type];

                            Texture2D texture = entry.texture2D;
                            Vector2 position = new Vector2(x * 16, y * 16);
                            Rectangle? source = default; // todo: figure this out
                            Color color = new Color(255, 255, 255);

                            // add SetDrawPositions()

                            // add DrawEffects() like tile has here?
                            spriteBatch.Draw(texture, position, source, color);
                            cable.PostDraw(spriteBatch);
                        }
                    }
                }
            }
        }
    }
}
