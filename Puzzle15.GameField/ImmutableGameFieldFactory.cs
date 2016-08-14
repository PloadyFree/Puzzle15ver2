﻿using System;
using System.Drawing;
using Puzzle15.Core.Arrays;

namespace Puzzle15.GameField
{
	public class ImmutableGameFieldFactory<TCell> : IGameFieldFactory<TCell>
	{
		public IGameField<TCell> CreateGameField(Size size, Func<CellLocation, TCell> getValue)
		{
			return new GameField<TCell>(size, getValue, true);
		}

		public IGameField<TCell> CreateGameField(IGameField<TCell> source) 
			=> CreateGameField(source.Size, loc => source[loc]);
	}
}