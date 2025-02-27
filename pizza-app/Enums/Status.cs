using System.Runtime.Serialization;

namespace pizza_app.Enums
{
    public enum CommandeStatus
    {
        [EnumMember(Value = "En cours")]
        EnCours,
        Livree,
        [EnumMember(Value = "En chemin")]
        EnChemin,
        A_Traiter
    }
}
