﻿using Ergo.Domain.Entities;
using Ergo.Domain.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ergo.Application.Features.Comments.Queries
{
    public class CommentDto
    {
        public Guid CommentId { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string? LastModifiedBy { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public string? CommentText { get; set; }
        public Guid TaskId { get; set; }

    }
}
