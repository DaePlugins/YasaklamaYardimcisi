using Rocket.API.Collections;
using Rocket.Core.Plugins;
using HarmonyLib;

namespace DaeYasaklamaYardimcisi
{
    public class YasaklamaYardımcısı : RocketPlugin<YasaklamaYardımcısıYapılandırma>
    {
        public static YasaklamaYardımcısı Örnek { get; private set; }
        private Harmony _harmony;

        protected override void Load()
        {
            Örnek = this;

            _harmony = new Harmony("dae.yasaklamayardimcisi");
            _harmony.PatchAll();
        }

        protected override void Unload()
        {
            Örnek = null;

            _harmony.UnpatchAll("dae.yasaklamayardimcisi");
            _harmony = null;
        }

        public override TranslationList DefaultTranslations => new TranslationList
        {
            { "OyuncuYasaklandı", "{0} SteamID64'üne sahip yasaklanan kullanıcının {1} adet barikatı, {2} adet yapısı ve {3} adet aracı silindi." }
        };
    }
}