using Models;

namespace CryptoCurrency
{
    internal static class MinerWallet
    {
        internal static void Load()
        {
            Miner.Wallet = Program.DomainName switch
            {
                "https://mysterious-thicket-34741.herokuapp.com/" => new()
                {
                    PublicKey = "1BPiqwmT9ig8cSfeRCiJaJU7qK4KrPKWhc",
                    PrivateKey = "L4fkiGDz1jdeTqo2rDUehWEWtDi3zhTnHwETi46zN9XGLoiAb9Rd",
                    BitcoinAddress = "1CvAdfEfhfhGSF8kbK7r2sB4DcKcSQi8GT"
                },
                "https://limitless-sands-00250.herokuapp.com/" => new()
                {
                    PublicKey = "1BwW9iTgFjsNPgE88b5kyowBQvGorHAux7",
                    PrivateKey = "KweNs6FRbt7UJDt73JZGqjyWrA543CT9Aqp3XDidTPJwGU2LGvuF",
                    BitcoinAddress = "1FCuTbUniYkLzZGt3sDeYFP3yVVst2CGQg"
                },
                "http://190.231.194.136/" => new()
                {
                    PublicKey = "1JMEnzGp35LzTyNJd4Bw1PuUDCx6pARDK7",
                    PrivateKey = "L3VJuTDuJPJNwwUEmSabco94YpmoxRp57np9BXLcGJk3VndoT5Uc",
                    BitcoinAddress = "17NjCw3UE2uuqytknBKJV6EV3akRGpK7hL"
                },
                "http://190.231.194.136:8081/" => new()
                {
                    PublicKey = "163jyvEH2YjfQrUcy51jY4h55sE75zoSdY",
                    PrivateKey = "L1gL7rvKT1rt2zq2zGB8XuALp3uSwB4uQ3n9YHng5jwCSJvrWPx1",
                    BitcoinAddress = "17UjZEZF7B9RRw4Ayqf77ExNagThX7G22N"
                },
                _ => new()
                {
                    PublicKey = "1MkTzCVgnDpnBueH2RmASj1aDPBgZDYLB",
                    PrivateKey = "L1pM5im8wCeiK9DMZctVL5FcqJcKrjvUQKW4PaeKFs8gvve4qo1K",
                    BitcoinAddress = "1857pVhG3wrbn6r8XrDKvy4fzBroR8kknY"
                }
            };
        }
    }
}
