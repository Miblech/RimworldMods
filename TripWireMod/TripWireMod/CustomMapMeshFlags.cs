using System; // Needed for the [Flags] attribute

namespace TripWireTrap
{
    // Define your custom MapMeshFlags here.
    // Use a high, unique ulong value to avoid conflicts with vanilla or other mods' flags.
    [Flags]
    public enum TripWireMeshFlag
    {
        None = 0x0,
        SecurityGrid = 0x400,
    }
}