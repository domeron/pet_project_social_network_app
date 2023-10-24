﻿using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SocialNetworkAppAPI.DTO;
using SocialNetworkAppLibrary.Exceptions;
using SocialNetworkAppAPI.Models;
using SocialNetworkAppLibrary.DTO;

namespace SocialNetworkAppAPI.Repositories;

public interface IPostRepository {
    IAsyncEnumerable<Post> GetPosts();
    Task<Post> CreatePostAsync(PostCreateDTO createModel);
    Task<Post> UpdatePostAsync(PostUpdateDTO updateModel);
    Task<Post> DeletePostAsync(int postId);
}
public class PostRepository : IPostRepository
{
    private readonly AppDbContext _context;

    public PostRepository(AppDbContext context)
    { 
        _context = context;
    }
    public async IAsyncEnumerable<Post> GetPosts()
    {
        var posts = _context.Posts.AsAsyncEnumerable();

        await foreach (var post in posts) { 
            yield return post;
        }
    }

    public async Task<Post> CreatePostAsync(PostCreateDTO model)
    {
        var post = new Post
        {
            Title = model.Title,
            Content = model.Content,
            LastModifiedDate = DateTime.UtcNow,
            CreatedDate = DateTime.UtcNow,
        };
        await _context.Posts.AddAsync(post);
        await _context.SaveChangesAsync();
        return post;
    }

    public async Task<Post> UpdatePostAsync(PostUpdateDTO model)
    {
        var post = await _context.Posts.Where(p => p.Id == model.Id).FirstOrDefaultAsync();
        if (post != null)
        {
            if (!model.Title.IsNullOrEmpty() && !post.Title.Equals(model.Title))
                post.Title = model.Title!;
            if(!model.Content.IsNullOrEmpty() && !post.Content.Equals(model.Content)) 
                post.Content = model.Content;

            post.LastModifiedDate = DateTime.UtcNow;
            _context.Entry(post).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return post;
        }
        else throw new NotFoundException();
    }

    public async Task<Post> DeletePostAsync(int postId)
    { 
        var post = await _context.Posts.Where(p => p.Id == postId).FirstOrDefaultAsync();
        if (post != null) { 
            _context.Posts.Remove(post);
            _context.Entry(post).State &= EntityState.Deleted;
            await _context.SaveChangesAsync();
            return post;
        } else throw new NotFoundException();
    }
}
