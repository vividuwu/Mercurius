using Mercurius.Configuration;
using System.Threading.Tasks;

namespace Mercurius.Profiles {
    public class Profile : IDisposable {
        // Make a reference to it's respective json file, with methods to update, delete, etc.
        public string Name { get; set; }
        public string MinecraftVersion { get; set; }
        public bool ServerSide { get; set; }
        public string Loader { get; set; }
        public bool ContainsUnknownMods = false;
        public List<Mod> Mods { get; set; }
        public UnknownMod[] UnknownMods = null;

        public string Path => string.Format("{0}\\{1}.profile.json", SettingsManager.Settings.Profile_Directory, Name); //"{SettingsManager.Settings.Profile_Directory}/{this.Name}.profile.json";
        private bool _disposed = false;

        public static async Task<Profile> CreateNewAsync(string name, string minecraftVersion, string loader, bool serverSide, bool select = false) {
            Profile profile = new Profile {
                Name = name,
                MinecraftVersion = minecraftVersion,
                ServerSide = serverSide,
                Loader = loader
            };
            await ProfileManager.WriteProfileAsync(profile);
            await ProfileManager.LoadProfileAsync(profile.Name);
            if (select) ProfileManager.SelectProfile(profile.Name);

            return profile;
        }
        public static async Task<Profile> CreateDefaultAsync(string name, string minecraftVersion) {
            Profile profile = new Profile {
                Name = name,
                MinecraftVersion = minecraftVersion,
                ServerSide = false,
                Loader = "fabric",
                Mods = new List<Mod>(),
                UnknownMods = null
            };
            await ProfileManager.WriteProfileAsync(profile);
            await ProfileManager.LoadProfileAsync(profile.Name);
            ProfileManager.SelectProfile(profile.Name);
            return profile;
        }

        public async Task<Profile> UpdateAsync(Profile oldProfile, Profile newProfile) {
            if (oldProfile.Equals(newProfile)) return oldProfile;

            await ProfileManager.OverwriteProfileAsync(newProfile, newProfile.Name);
            return await ProfileManager.LoadProfileAsync(newProfile.Name);
        }
        public async Task UpdateModListAsync(List<Mod> mods) {
            foreach (Mod mod in mods)
                Mods.Add(mod);

            await ProfileManager.OverwriteProfileAsync(this, this.Name);
        }
        public async Task UpdateModListAsync(Mod mod) {
                Mods.Add(mod);

            await ProfileManager.OverwriteProfileAsync(this, this.Name);
        }
        public void Delete() {
            // Dipose/unload
            ProfileManager.UnloadProfile(this);

            if (File.Exists(Path))
                ProfileManager.DeleteProfileFile(Name);
        }

        public void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        private void Dispose(bool disposing) {
            if (_disposed) return;
            if (disposing) {
                ProfileManager.UnloadProfile(this);
            }

            _disposed = true;
        }
    }
    public enum ClientType {
        ClientSide, ServerSide
    }
}