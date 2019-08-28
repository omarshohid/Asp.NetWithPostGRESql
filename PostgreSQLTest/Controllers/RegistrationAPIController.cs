using DAL.Model;
using DAL.Repository;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Web.Http;

namespace PostgreSQLTest.Controllers
{
    public class RegistrationAPIController : ApiController
    {
        UnitOfWork unitOfWork = new UnitOfWork();


        [AcceptVerbs("POST")]
        [HttpPost, ActionName("AddNewUser")]
        public IHttpActionResult AddNewUser(AspNetUsers user)
        {
            try
            {
                if(user != null)
                {
                    user.id = Guid.NewGuid().ToString();
                    user.password_hash = Encrypt(user.password);
                    unitOfWork.AspNetUsersRepository.Insert(user);
                    unitOfWork.Save();
                    return Ok("Data Saved Succesfully");
                }
                else
                {
                    return NotFound();
                }
                
            }
            catch (Exception ex)
            {
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        [AcceptVerbs("POST")]
        [HttpPost, ActionName("Login")]
        public IHttpActionResult Login(AspNetUsers user)
        {
            try
            {
                if (user != null)
                {
                    var userInfo = unitOfWork.AspNetUsersRepository.Get().FirstOrDefault(x => x.username == user.username);
                    if(userInfo != null)
                    {
                        var password = Decrypt(userInfo.password_hash);
                        if(user.password == password)
                        {
                            return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.OK, "Succesfully Loged in"));
                        }
                        else
                        {
                            return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.NotFound, "Invalid Username or Password!"));
                        }
                    }
                    else
                    {
                        return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.NotFound, "Invalid Username or Password!"));
                    }             

                }
                else
                {
                    return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.NotFound, "No data found!"));
                }

            }
            catch (Exception ex)
            {
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }














































































































































        static readonly string PasswordHash = "P@@Sw0rd";
        static readonly string SaltKey = "S@LT&KEY";
        static readonly string VIKey = "@1B2c3D4e5F6g7H8";
        public static string Encrypt(string plainText)
        {
            byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);

            byte[] keyBytes = new Rfc2898DeriveBytes(PasswordHash, Encoding.ASCII.GetBytes(SaltKey)).GetBytes(256 / 8);
            var symmetricKey = new RijndaelManaged() { Mode = CipherMode.CBC, Padding = PaddingMode.Zeros };
            var encryptor = symmetricKey.CreateEncryptor(keyBytes, Encoding.ASCII.GetBytes(VIKey));

            byte[] cipherTextBytes;

            using (var memoryStream = new MemoryStream())
            {
                using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                {
                    cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
                    cryptoStream.FlushFinalBlock();
                    cipherTextBytes = memoryStream.ToArray();
                    cryptoStream.Close();
                }
                memoryStream.Close();
            }
            return Convert.ToBase64String(cipherTextBytes);
        }

        public static string Decrypt(string encryptedText)
        {
            byte[] cipherTextBytes = Convert.FromBase64String(encryptedText);
            byte[] keyBytes = new Rfc2898DeriveBytes(PasswordHash, Encoding.ASCII.GetBytes(SaltKey)).GetBytes(256 / 8);
            var symmetricKey = new RijndaelManaged() { Mode = CipherMode.CBC, Padding = PaddingMode.None };

            var decryptor = symmetricKey.CreateDecryptor(keyBytes, Encoding.ASCII.GetBytes(VIKey));
            var memoryStream = new MemoryStream(cipherTextBytes);
            var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
            byte[] plainTextBytes = new byte[cipherTextBytes.Length];

            int decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
            memoryStream.Close();
            cryptoStream.Close();
            return Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount).TrimEnd("\0".ToCharArray());
        }
    }


}
