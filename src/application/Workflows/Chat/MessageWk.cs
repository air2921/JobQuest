﻿using AutoMapper;
using common.DTO.ModelDTO;
using common.DTO.ModelDTO.Chat;
using common.Exceptions;
using domain.Abstractions;
using domain.Includes;
using domain.Localize;
using domain.Models.Chat;
using domain.SpecDTO;
using domain.Specifications.Message;
using JsonLocalizer;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace application.Workflows.Chat;

public class MessageWk(
    IRepository<MessageModel> repository,
    IRepository<ChatModel> chatRepository,
    IDatabaseTransaction databaseTransaction,
    IMapper mapper,
    ILocalizer localizer) : Responder
{
    public async Task<Response> GetRange(PaginationDTO dto, int chatId, int userId, bool? onlyRead, string? keyword)
    {
        try
        {
            var spec = new SortMessageSpec(dto.Skip, dto.Total, dto.ByDesc, chatId, userId) { OnlyRead = onlyRead, KeyWord = keyword };
            var include = new MessageInclude { IncludeChat = true };
            var messages = await repository.GetRangeAsync(spec, include);
            if (messages is null)
                return Response(404, localizer.Translate(Messages.NOT_FOUND));

            return Response(200, new { messages });
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
            var spec = new MessageByIdSpec(id);
            var include = new MessageInclude { IncludeChat = true };
            var message = await repository.GetByIdWithInclude(spec, include);
            if (message is null || message.EmployerId != userId || message.CandidateId != userId)
                return Response(404, localizer.Translate(Messages.NOT_FOUND));

            return Response(200, new { message });
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
            var message = await repository.DeleteAsync(id);
            if (message is null || message.EmployerId != userId || message.CandidateId != userId)
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

    public async Task<Response> AddSingle(MessageDTO dto)
    {
        try
        {
            var model = mapper.Map<MessageModel>(dto);
            await repository.AddAsync(model);
            var chat = await chatRepository.GetByIdAsync(model.ChatId);
            if (chat is not null)
                model.Chat = chat;
            return Response(201, new { model });
        }
        catch (EntityException ex)
        {
            return Response(500, ex.Message);
        }
    }

    public async Task<Response> Update(MessageDTO dto, int messageId, int userId)
    {
        try
        {
            var entity = await repository.GetByIdAsync(messageId);
            if (entity is null || entity.CandidateId != userId || entity.EmployerId != userId)
                return Response(404, localizer.Translate(Messages.NOT_FOUND));

            entity = mapper.Map(dto, entity);
            await repository.UpdateAsync(entity);
            return Response(200, new { entity });
        }
        catch (EntityException ex)
        {
            return Response(500, ex.Message);
        }
    }
}
