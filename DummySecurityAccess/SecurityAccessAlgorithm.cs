using SecurityAccessContract;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DummySecurityAccess
{
    [Export(typeof(ISecurityAccessAlgorithm))]
    public class SecurityAccessAlgorithm : ISecurityAccessAlgorithm
    {
        public const int ExtendSessionLevel = 0x01;

        public string Name => "Dummy";

        public List<byte> Encrypt(int securityLevel, List<byte> rawData)
        {
            if (securityLevel % 2 == 0)
            {
                securityLevel -= 1;
            }

            if (securityLevel < 0)
            {
                throw new ArgumentException("security level must be great than 0");
            }

            if (ExtendSessionLevel == securityLevel)
            {
                return ExtendSessionLevelEncrypt(rawData);
            }
            else
            {
                throw new ArgumentException("Unsupported security level " + securityLevel.ToString());
            }
        }

        private List<byte> ExtendSessionLevelEncrypt(List<byte> seed)
        {
            if (seed.Count != 4)
            {
                throw new ArgumentException("seed data length must be 4");
            }

            List<byte> key = new List<byte>();
            const byte increVal = 0x55;

            foreach (var s in seed)
            {
                key.Add((byte)(increVal + s));
            }

            return key;
        }
    }
}
