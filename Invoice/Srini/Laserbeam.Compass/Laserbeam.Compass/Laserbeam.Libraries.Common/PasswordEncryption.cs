// Copyright (c) 2015 Laserbeam Software Pvt.ltd.  All rights reserved.
// Confidential and proprietary.

// Component Name  : 	PasswordEncryption
// Description     : 	Encrypt and Validate password	
// Author          :	Roopan		
// Creation Date   : 	APR-09-2015

using Laserbeam.Libraries.Interfaces.Common;
using System;
using System.Security.Cryptography;

namespace Laserbeam.Libraries.Common
{
        public class PasswordEncryption : IPasswordEncryption
        {
            // The following constants may be changed without breaking existing hashes.
            private const int SALT_BYTE_SIZE = 26;
            private const int HASH_BYTE_SIZE = 26;
            private const int PBKDF2_ITERATIONS = 1500;

            private const int ITERATION_INDEX = 1;
            private const int SALT_INDEX = 0;
            private const int PBKDF2_INDEX = 2;
            private char delimiter = '@';

            // Author        :  Roopan		
            // Creation Date :  APR-13-2015
            /// <summary>
            /// Creates a salted PBKDF2 hash of the password.
            /// </summary>
            /// <param name="password">The password to hash.</param>
            /// <returns>The hash of the password.</returns>
            public string EncryptPassword(string password)
            {
                // Generate a random salt
                RNGCryptoServiceProvider csprng = new RNGCryptoServiceProvider();
                byte[] salt = new byte[SALT_BYTE_SIZE];
                csprng.GetBytes(salt);

                // Hash the password and encode the parameters
                byte[] hash = PBKDF2(password, salt, PBKDF2_ITERATIONS, HASH_BYTE_SIZE);
                return  generateHashPassword(salt,hash);
            }

            // Author        :  Roopan		
            // Creation Date :  APR-13-2015
            /// <summary>
            /// Validates a password given a hash of the correct one.
            /// </summary>
            /// <param name="password">The password to check.</param>
            /// <param name="userPassword">A hash of the user password.</param>
            /// <returns>True if the password is correct. False otherwise.</returns>
            public bool ValidatePassword(string password, string userPassword)
            {
                // Extract the parameters from the hash
                if (userPassword != null && password != null)
                {
                    string[] split = userPassword.Split(delimiter);
                    int iterations = Int32.Parse(split[ITERATION_INDEX]);
                    byte[] salt = Convert.FromBase64String(split[SALT_INDEX]);
                    byte[] hash = Convert.FromBase64String(split[PBKDF2_INDEX]);
                    byte[] userHash = PBKDF2(password, salt, iterations, hash.Length);
                    return slowEquals(hash, userHash);
                }
                return false;
            }

            // Author        :  Roopan		
            // Creation Date :  APR-13-2015
            /// <summary>
            /// Compares two byte arrays in length-constant time. This comparison
            /// method is used so that password hashes cannot be extracted from
            /// on-line systems using a timing attack and then attacked off-line.
            /// </summary>
            /// <param name="a">The first byte array.</param>
            /// <param name="b">The second byte array.</param>
            /// <returns>True if both byte arrays are equal. False otherwise.</returns>
            private bool slowEquals(byte[] a, byte[] b)
            {
                uint diff = (uint)a.Length ^ (uint)b.Length;
                for (int i = 0; i < a.Length && i < b.Length; i++)
                    diff |= (uint)(a[i] ^ b[i]);
                return diff == 0;
            }

            // Author        :  Roopan		
            // Creation Date :  APR-13-2015
            /// <summary>
            /// Computes the PBKDF2-SHA1 hash of a password.
            /// </summary>
            /// <param name="password">The password to hash.</param>
            /// <param name="salt">The salt.</param>
            /// <param name="iterations">The PBKDF2 iteration count.</param>
            /// <param name="outputBytes">The length of the hash to generate, in bytes.</param>
            /// <returns>A hash of the password.</returns>
            private byte[] PBKDF2(string password, byte[] salt, int iterations, int outputBytes)
            {
                Rfc2898DeriveBytes pbkdf2 = new Rfc2898DeriveBytes(password, salt);
                pbkdf2.IterationCount = iterations;
                return pbkdf2.GetBytes(outputBytes);
            }

            // Author        :  Roopan		
            // Creation Date :  APR-13-2015
            /// <summary>
            /// Generate haseh password based on configured index
            /// </summary>
            /// <param name="salt">The salt.</param>
            /// <param name="hash">The hash.</param>
            /// <returns></returns>
            private string generateHashPassword(byte[] salt, byte[] hash)
            {
                string encrypt = String.Empty;
                for (int i = 0; i < 3; i++)
                {
                    if(i!=0)
                        encrypt = encrypt + delimiter;
                    switch (i)
                    {
                        case ITERATION_INDEX:
                            encrypt =encrypt+ Convert.ToString(PBKDF2_ITERATIONS);
                            break;
                        case SALT_INDEX:
                            encrypt = encrypt + Convert.ToBase64String(salt);
                            break;
                        case PBKDF2_INDEX:
                            encrypt = encrypt + Convert.ToBase64String(hash);
                            break;
                    }                   
                }
                return encrypt;
            }
        }
}
