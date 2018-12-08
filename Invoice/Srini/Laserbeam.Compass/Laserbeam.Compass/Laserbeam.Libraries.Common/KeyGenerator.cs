// Copyright (c) 2015 Laserbeam Software Pvt.ltd.  All rights reserved.
// Confidential and proprietary.

// Component Name  : 	KeyGenerator
// Description     : 	To generate random key	
// Author          :	Roopan		
// Creation Date   : 	APR-09-2015

using Laserbeam.Libraries.Interfaces.Common;
using System.Security.Cryptography;
using System.Text;

namespace Laserbeam.Libraries.Common
{
    public class KeyGenerator : IKeyGenerator
    {
        // Author        :  Roopan		
        // Creation Date :  APR-09-2015
       /// <summary>
       /// Generates a random key for given length
       /// </summary>
       /// <param name="maxSize">length of key to generate</param>
       /// <returns></returns>
        public string GenerateRandomKey(int maxSize)
        {
            char[] chars = new char[62];
            chars =
            "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890".ToCharArray();
            byte[] data = new byte[1];
            using (RNGCryptoServiceProvider crypto = new RNGCryptoServiceProvider())
            {
                crypto.GetNonZeroBytes(data);
                data = new byte[maxSize];
                crypto.GetNonZeroBytes(data);
            }
            StringBuilder result = new StringBuilder(maxSize);
            foreach (byte b in data)
            {
                result.Append(chars[b % (chars.Length)]);
            }
            return result.ToString();
        }
    }
}
