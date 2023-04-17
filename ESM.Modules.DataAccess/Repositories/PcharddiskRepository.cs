﻿using ESM.Modules.DataAccess.DTOs;
using ESM.Modules.DataAccess.Infrastructure;
using ESM.Modules.DataAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace ESM.Modules.DataAccess.Repositories;

public interface IPcharddiskRepository : IProductRepository<Pcharddisk>
{
}
public class PcharddiskRepository : ProductRepository<Pcharddisk>, IPcharddiskRepository
{
    public PcharddiskRepository(ESMDbContext context) : base(context)
    {
    }
    public override async Task<bool> IsIdExist(string id)
    {
        return await _context.Pcharddisks.AnyAsync(x => x.Id == id);
    }
    public override async Task<Pcharddisk?> GetById(string id)
    {
        return await _context.Pcharddisks.AsQueryable()
                .Where(p => p.Id == id)
                .FirstOrDefaultAsync();
    }
    public override async Task<object?> Update(Pcharddisk entity)
    {
        bool res = true;
        try
        {
            var hd = await _context.Pcharddisks.AsQueryable()
                   .FirstAsync(p => p.Id == entity.Id);
            _context.Entry(hd).CurrentValues.SetValues(entity);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            res = false;
        }
        return res;
    }
    public override async Task<IEnumerable<Pcharddisk>?> GetAll()
    {
        return await _context.Pcharddisks.AsQueryable()
                .Where(pcharddisk => pcharddisk.Remain > -1)
                .ToListAsync();
    }
    public override async Task<object?> Add(Pcharddisk entity)
    {
        await _context.Pcharddisks.AddAsync(entity);
        await _context.SaveChangesAsync();
        return null;
    }
    public override async Task<object?> Delete(string id)
    {
        var p = await _context.Pcharddisks.SingleAsync(p => p.Id == id);
        p.Remain = -1;
        await _context.SaveChangesAsync();
        return null;
    }
    public override async Task<object?> AddList(IEnumerable<Pcharddisk> list)
    {
        bool res = true;
        try
        {
            await _context.Pcharddisks.AddRangeAsync(list);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex) { res = false; }
        return res;
    }
    public string GetSuggestID()
    {
        return GetSuggestID(ProductType.HARDDISK);
    }
}
