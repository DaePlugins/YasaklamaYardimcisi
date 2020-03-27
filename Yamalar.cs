using System.Linq;
using Rocket.Unturned.Chat;
using SDG.Unturned;
using Steamworks;
using UnityEngine;
using HarmonyLib;
using Math = System.Math;

namespace DaeYasaklamaYardimcisi
{
    [HarmonyPatch(typeof(SteamBlacklist))]
    [HarmonyPatch("ban")]
    [HarmonyPatch(new[]{ typeof(CSteamID), typeof(uint), typeof(CSteamID), typeof(string), typeof(uint) })]
    internal class Yamalar
    {
        [HarmonyPostfix]
        private static void YasaklamadanSonra(CSteamID playerID)
        {
            var steamId = playerID.m_SteamID;

            var silinenAraçSayısı = 0;
            if (YasaklamaYardımcısı.Örnek.Configuration.Instance.AraçlarSilinsin)
            {
                var araçlar = VehicleManager.vehicles.Where(a => a.lockedOwner.m_SteamID == steamId).ToList();

                for (var i = araçlar.Count - 1; i >= 0; i--)
                {
                    var araç = araçlar[i];
                    araç.forceRemoveAllPlayers();
                    VehicleManager.instance.channel.send("tellVehicleDestroy", ESteamCall.ALL, ESteamPacket.UPDATE_RELIABLE_BUFFER, araç.instanceID);

                    silinenAraçSayısı++;
                }
            }

			var silinenBarikatSayısı = 0;
            if (YasaklamaYardımcısı.Örnek.Configuration.Instance.BarikatlarSilinsin)
			{
			    for (byte x = 0; x < BarricadeManager.BarricadeRegions.GetLength(0); x++)
			    {
			        for (byte y = 0; y < BarricadeManager.BarricadeRegions.GetLength(1); y++)
			        {
			            var barikatBölgesi = BarricadeManager.BarricadeRegions[x, y];
                        
			            for (var sıra = barikatBölgesi.drops.Count - 1; sıra >= 0; sıra--)
			            {
			                if (barikatBölgesi.barricades[sıra].owner != steamId)
			                {
			                    continue;
			                }

			                barikatBölgesi.barricades.RemoveAt(sıra);
			                BarricadeManager.instance.channel.send("tellTakeBarricade", ESteamCall.ALL, x, y, BarricadeManager.BARRICADE_REGIONS, ESteamPacket.UPDATE_RELIABLE_BUFFER, x, y, 65535, sıra);

			                silinenBarikatSayısı++;
			            }
			        }
			    }
                
			    for (var plant = BarricadeManager.plants.Count - 1; plant >= 0; plant--)
			    {
			        var barikatBölgesi = BarricadeManager.plants[plant];

			        for (var sıra = barikatBölgesi.drops.Count - 1; sıra >= 0; sıra--)
			        {
			            if (barikatBölgesi.barricades[sıra].owner != steamId)
			            {
			                continue;
			            }

			            barikatBölgesi.barricades.RemoveAt(sıra);
			            BarricadeManager.instance.channel.send("tellTakeBarricade", ESteamCall.ALL, ESteamPacket.UPDATE_RELIABLE_BUFFER, byte.MinValue, byte.MinValue, (ushort)Math.Min(plant, ushort.MaxValue), sıra);

			            silinenBarikatSayısı++;
			        }
			    }
			}

			var silinenYapıSayısı = 0;
            if (YasaklamaYardımcısı.Örnek.Configuration.Instance.YapılarSilinsin)
			{
			    for (byte x = 0; x < StructureManager.regions.GetLength(0); x++)
			    {
			        for (byte y = 0; y < StructureManager.regions.GetLength(1); y++)
			        {
			            var yapıBölgesi = StructureManager.regions[x, y];
                        
			            for (var sıra = yapıBölgesi.drops.Count - 1; sıra >= 0; sıra--)
			            {
			                if (yapıBölgesi.structures[sıra].owner != steamId)
			                {
			                    continue;
			                }

			                yapıBölgesi.structures.RemoveAt(sıra);
			                StructureManager.instance.channel.send("tellTakeStructure", ESteamCall.ALL, x, y, StructureManager.STRUCTURE_REGIONS, ESteamPacket.UPDATE_RELIABLE_BUFFER, x, y, sıra, new Vector3(0, 1024, 0));

			                silinenYapıSayısı++;
			            }
			        }
			    }
			}

			UnturnedChat.Say(YasaklamaYardımcısı.Örnek.Translate("OyuncuYasaklandı", steamId, silinenBarikatSayısı, silinenYapıSayısı, silinenAraçSayısı));
        }
    }
}