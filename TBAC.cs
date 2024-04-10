using System.IO;
using TBAC.Content.Systems;
using Terraria.ModLoader;

namespace TBAC
{
	public partial class TBAC : Mod
	{
		public const string AssetPath = "TBAC/Assets/";

		public const string TextureHoldplacer = AssetPath + "Textures/Holdplacer";

        public override void Load()
        {
            base.Load();

            StandLoader.Load();
        }

        public override void Unload()
        {
            base.Unload();

            StandLoader.Unload();
        }
    }
}