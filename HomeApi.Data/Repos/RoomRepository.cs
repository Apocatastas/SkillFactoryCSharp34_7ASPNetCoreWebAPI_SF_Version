﻿using System;
using System.Linq;
using System.Threading.Tasks;
using HomeApi.Data.Models;
using HomeApi.Data.Queries;
using Microsoft.EntityFrameworkCore;

namespace HomeApi.Data.Repos
{
    /// <summary>
    /// Репозиторий для операций с объектами типа "Room" в базе
    /// </summary>
    public class RoomRepository : IRoomRepository
    {
        private readonly HomeApiContext _context;
        
        public RoomRepository (HomeApiContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Выгрузить все комнаты
        /// </summary>
        public async Task<Room[]> GetRooms()
        {
            return await _context.Rooms
                .ToArrayAsync();
        }

        /// <summary>
        /// Найти комнату по идентификатору
        /// </summary>
        public async Task<Room> GetRoomById(Guid id)
        {
            return await _context.Rooms
                .Where(d => d.Id == id).FirstOrDefaultAsync();
        }


        /// <summary>
        ///  Найти комнату по имени
        /// </summary>
        public async Task<Room> GetRoomByName(string name)
        {
            return await _context.Rooms.Where(r => r.Name == name).FirstOrDefaultAsync();
        }
        
        /// <summary>
        ///  Добавить новую комнату
        /// </summary>
        public async Task AddRoom(Room room)
        {
            var entry = _context.Entry(room);
            if (entry.State == EntityState.Detached)
                await _context.Rooms.AddAsync(room);
            
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Обновить существующее комнату
        /// </summary>
        public async Task UpdateRoom(Room room, UpdateRoomQuery query)
        {
            if (!string.IsNullOrEmpty(query.NewName))
                room.Name = query.NewName;
            if (query.NewArea != room.Area && query.NewArea != 0)
                room.Area = query.NewArea;
            room.GasConnected = query.NewGasConnected;
            if (query.NewVoltage != room.Voltage && query.NewVoltage != 0)
                room.Voltage = query.NewVoltage;

            var entry = _context.Entry(room);
            if (entry.State == EntityState.Detached)
                _context.Rooms.Update(room);

            await _context.SaveChangesAsync();
        }
    }
}