namespace Mercurius.Profiles {
    public class Profile {
        public string Name { get; set; }
        public string MinecraftVersion { get; set; }
        public ClientType ClientType { get; set; }
        public bool ContainsUnknownMods = false;
        public Mod[] Mods { get; set; }
        public UnknownMod[] UnknownMods = null;
    }
    public enum ClientType {
        ClientSideRequired, serverSideRequired, ClientServerDependent
    }
}