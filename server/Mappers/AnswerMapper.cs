using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using server.DTOs.Answer;
using server.DTOs.Users;
using server.Models;

namespace server.Mappers
{
    public static class AnswerMapper
    {
        public static AnswerDto toAnswerDto(this Answer answer, UserDto user)
        {
            return new AnswerDto{
                Id = answer.Id,
                Content = answer.Content,
                PostTime = answer.PostTime,
                CommentId = answer.CommentId,
                EditedTime = answer.EditedTime,
                User = user
            };
        }

        public static Answer fromCreateDtoToAnswer(this CreateAnswerDto dto)
        {
            return new Answer{
                EditedTime = DateTime.Now,
                PostTime = DateTime.Now,
                Content = dto.Content,
                CommentId = dto.CommentId,
                UserId = dto.UserId
            };
        }
    }
}