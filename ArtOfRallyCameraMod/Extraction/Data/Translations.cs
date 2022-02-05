using HarmonyLib;
using I2.Loc;

namespace ArtOfRallyChampionshipMod.Extraction.Data
{
    [HarmonyPatch(typeof(LocalizationManager), "AddSource")]
    public class AddSource
    {
        public static void Postfix(LanguageSourceData __0)
        {
            Main.Client.EmitAsync("translationSourceAdded", Newtonsoft.Json.JsonConvert.SerializeObject(
                __0));
        }
    }
}