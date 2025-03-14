﻿using System.Data;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using TheBillboard.Abstract;
using TheBillboard.Models;
using TheBillboard.Options;

namespace TheBillboard.Readers;

public class SQLReader : IReader
{
    private readonly string _connectionString;

    public SQLReader(IOptions<ConnectionStringOptions> options)
    {
        _connectionString = options.Value.DefaultDatabase;
    }

    public async IAsyncEnumerable<TEntity> QueryAsync<TEntity>(string query, Func<IDataReader, TEntity> selector)
    {


        await using var connection = new SqlConnection(_connectionString);

        await using SqlCommand? command = new(query, connection);

        await connection.OpenAsync();
        await using var dr = command.ExecuteReader();
        while (await dr.ReadAsync())
        {
            var message = selector(dr);
            yield return message;
        }

        await connection.CloseAsync();
        await connection.DisposeAsync();

        
    }

    public async Task<TEntity?> QueryOnceAsync<TEntity>(string query, Func<IDataReader, TEntity> selector)
    {
        TEntity? message = default;

        await using var connection = new SqlConnection(_connectionString);
        await using var command = new SqlCommand(query, connection);

        await connection.OpenAsync();
        await using var dr = command.ExecuteReader();
        //Only read one row
        if (await dr.ReadAsync())
        {
            message = selector(dr);
        }

        await connection.CloseAsync();
        await connection.DisposeAsync();

        return message;
    }

    public async Task<TEntity?> QueryByIdAsync<TEntity>(string query, Func<IDataReader, TEntity> selector, int id)
    {
        TEntity? message = default;

        await using var connection = new SqlConnection(_connectionString);

        await using var command = new SqlCommand(query, connection);

        var idParameter = new SqlParameter("@id", id);

        command.Parameters.Add(idParameter);

        await connection.OpenAsync();
        //await command.PrepareAsync();
        await using var dr = command.ExecuteReader();
        //Only read one row
        if (await dr.ReadAsync())
        {
            message = selector(dr);
        }

        await connection.CloseAsync();
        await connection.DisposeAsync();

        return message;
    }
}