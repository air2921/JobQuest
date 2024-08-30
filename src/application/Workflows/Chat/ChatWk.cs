using AutoMapper;
using common.DTO.ModelDTO.Chat;
using common.Exceptions;
using domain.Abstractions;
using domain.Localize;
using domain.Models.Chat;
using domain.SpecDTO;
using domain.Specifications.Chat;
using JsonLocalizer;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace application.Workflows.Chat;

public class ChatWk(
    IRepository<ChatModel> repository,
    IDatabaseTransaction databaseTransaction,
    IMapper mapper,
    ILocalizer localizer) : Responder
{
    public async Task<Response> GetRange(PaginationDTO dto, int userId)
    {
        try
        {
            var chats = await repository.GetRangeAsync(new SortChatSpec(dto.Skip, dto.Total, dto.ByDesc, userId));
            if (chats is null)
                return Response(404, localizer.Translate(Messages.NOT_FOUND));

            return Response(200, new { chats });
        }
        catch (EntityException ex)
        {
            return Response(500, ex.Message);
        }
    }

    public async Task<Response> GetSingle(int id, int userId)
    {
        try
        {
            var chat = await repository.GetByIdAsync(id);
            if (chat is null || chat.EmployerId != userId || chat.CandidateId != userId)
                return Response(404, localizer.Translate(Messages.NOT_FOUND));

            return Response(200, new { chat });
        }
        catch (EntityException ex)
        {
            return Response(500, ex.Message);
        }
    }

    public async Task<Response> RemoveSingle(int id, int userId)
    {
        using var transaction = databaseTransaction.Begin();

        try
        {
            var chat = await repository.DeleteAsync(id);
            if (chat is null || chat.EmployerId != userId || chat.CandidateId != userId)
            {
                transaction.Rollback();
                return Response(404, localizer.Translate(Messages.NOT_FOUND));
            }

            transaction.Commit();
            return Response(204);
        }
        catch (EntityException ex)
        {
            transaction.Rollback();
            return Response(500, ex.Message);
        }
    }

    public async Task<Response> RemoveRange(IEnumerable<int> identifiers, int userId)
    {
        using var transaction = databaseTransaction.Begin();

        try
        {
            var entities = await repository.DeleteRangeAsync(identifiers);
            if (entities.Any(e => e is null || e.EmployerId != userId || e.CandidateId != userId))
            {
                transaction.Rollback();
                return Response(403, localizer.Translate(Messages.FORBIDDEN));
            }

            transaction.Commit();
            return Response(204);
        }
        catch (EntityException ex)
        {
            transaction.Rollback();
            return Response(500, ex.Message);
        }
    }

    public async Task<Response> AddSingle(ChatDTO dto)
    {
        try
        {
            var model = mapper.Map<ChatModel>(dto);
            await repository.AddAsync(model);
            return Response(201);
        }
        catch (EntityException ex)
        {
            return Response(500, ex.Message);
        }
    }

    public async Task<Response> AddRange(IEnumerable<ChatDTO> dtos)
    {
        try
        {
            var entities = new List<ChatModel>();

            foreach (var dto in dtos)
            {
                var model = mapper.Map<ChatModel>(dto);
                entities.Add(model);
            }

            await repository.AddRangeAsync(entities);
            return Response(201);
        }
        catch (EntityException ex)
        {
            return Response(500, ex.Message);
        }
    }
}
