using Rocket.API;

namespace DaeYasaklamaYardimcisi
{
    public class YasaklamaYardımcısıYapılandırma : IRocketPluginConfiguration
    {
        public bool BarikatlarSilinsin { get; set; }
        public bool YapılarSilinsin { get; set; }
        public bool AraçlarSilinsin { get; set; }

        public void LoadDefaults()
        {
            BarikatlarSilinsin = true;
            YapılarSilinsin = true;
            AraçlarSilinsin = true;
        }
    }
}