using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;

namespace TBAC.Core.UI
{
    [Autoload(Side = ModSide.Client)]
    public class UISystem : ModSystem
    {
        private UserInterface abilityMenuInterface;
        internal AbilityUIState abilityState;
        public override void Load()
        {
            base.Load();

            abilityMenuInterface = new UserInterface();

            abilityState = new AbilityUIState();

            abilityMenuInterface.SetState(abilityState);
            abilityState.Activate();
        }

        public override void UpdateUI(GameTime gameTime)
        {
            base.UpdateUI(gameTime);

            abilityState.Update(gameTime);
        }

        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            int mouseIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Mouse Text"));
            if (mouseIndex != -1) {
                layers.Insert(mouseIndex, new LegacyGameInterfaceLayer
                    ("TBAF: Ability UI",
                    delegate
                    {
                        if (abilityMenuInterface?.CurrentState != null) {
                            abilityMenuInterface.Draw(Main.spriteBatch, new GameTime());
                        }
                        return true;
                    }, InterfaceScaleType.UI));
            }
        }
    }
}
