using System;

namespace WebApi.Models.Completation
{
    public class CompletationPalletPackAction
    {
        int id { get; set; }
        int serialNumber{ get; set; }
        int operatorId { get; set; }
        DateTime timestamp { get; set; }
    }
}
