﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pressford.News.Model;

namespace Pressford.News.Services
{
    public interface IDashboardService
    {
        Task<List<Article>> GetPublishedArticles();

        Task<List<Article>> GetAllPublishedArticle();
    }
}