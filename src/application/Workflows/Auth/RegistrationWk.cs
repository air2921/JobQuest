using application.Abstractions.Infrastructure;
using application.DTO.Api;
using domain.Abstractions;
using domain.Exceptions;
using domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace application.Workflows.Auth
{
    //public class RegistrationWk(
    //    IRepository<UserModel> userRepo,
    //    IDataCache dataCache,
    //    IGenerate generate,
    //    IHashUtility hashUtility)
    //{
    //    public async Task<Response> Registration(RegisterDTO dto)
    //    {
    //        try
    //        {
    //            dto.Email = dto.Email.ToLowerInvariant();
    //            var code = generate.GenerateCode(6);
    //        }
    //        catch (EntityException ex)
    //        {

    //        }
    //        catch (SmtpClientException ex)
    //        {

    //        }
    //    }

    //    public async Task<Response> Verify(string email, int code)
    //    {

    //    }
    //}
}
