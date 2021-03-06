﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Puzzle15.Core.Arrays;

namespace Puzzle15.GameField
{
	public abstract class GameFieldBase<TCell> : IGameField<TCell>
	{
		public Size Size { get; }
		public bool Immutable { get; }
		public TCell EmptyCellValue => default(TCell);

		protected GameFieldBase(Size size, bool immutable)
		{
			Size = size;
			Immutable = immutable;
		}

		protected void CheckLocation(CellLocation location)
		{
			if (!location.IsInsideField(Size))
				throw new InvalidLocationException();
		}

		protected void CheckDefaultValuesCount()
		{
			var count = this.Count(x => IsEmptyValue(x.Value));
			if (count != 1)
				throw new InvalidOperationException($"Field should contain exactly 1 default value, but found {count}");
		}
		
		protected bool IsEmptyValue(TCell value) => Equals(value, EmptyCellValue);

		public abstract IGameField<TCell> Shift(TCell value);
		public abstract IGameField<TCell> Shift(CellLocation valueLocation);

		public bool Contains(CellLocation location)
		{
			var row = location.Row;
			var col = location.Column;

			return 0 <= row && row < Size.Height &&
			       0 <= col && col < Size.Width;
		}

		public abstract IEnumerable<CellLocation> GetLocations(TCell value);

		public virtual CellLocation GetLocation(TCell value)
		{
			var locations = GetLocations(value).ToList();
			if (locations.Count != 1)
				throw new ArgumentException($"Values count on the field should be 1, but now is {locations.Count}");
			return locations.Single();
		}

		public abstract TCell this[CellLocation location] { get; }

		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

		public abstract IEnumerator<CellInfo<TCell>> GetEnumerator();

		public abstract IGameField<TCell> Clone();

		public bool Equals(IGameField<TCell> other)
		{
			return other != null &&
			       Equals(Size, other.Size) &&
			       this.SequenceEqual(other);
		}

		public override bool Equals(object obj)
		{
			return Equals(obj as IGameField<TCell>);
		}

		public override int GetHashCode()
		{
			return 0;
		}

		public override string ToString()
		{
			var lines = Enumerable.Range(0, Size.Height)
				.Select(i => Enumerable.Range(0, Size.Width)
					.Select(j => this[new CellLocation(i, j)]));
			var joinedLines = lines.Select(line => string.Join(" ", line));
			return string.Join("\n", joinedLines);
		}
	}
}