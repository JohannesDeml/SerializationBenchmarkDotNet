// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ATestTarget.cs">
//   Copyright (c) 2020 Johannes Deml. All rights reserved.
// </copyright>
// <author>
//   Johannes Deml
//   public@deml.io
// </author>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;

namespace DotNetSerializationBenchmark
{
	public class TestResult
	{
		public object Result;
		public long ByteSize;

		public TestResult(object result, long byteSize)
		{
			Result = result;
			ByteSize = byteSize;
		}
	}

	public abstract class ASerializerTarget
	{
		private Dictionary<Type, TestResult> results;
		protected ASerializerTarget()
		{
			results = new Dictionary<Type, TestResult>();
		}

		public long Run<T>(T original)
		{
			var messageSize = Serialize(original);

			var copy = Deserialize<T>();
			results[typeof(T)] = new TestResult(copy, messageSize);

			return messageSize;
		}

		public bool Validate<T>(T original) where T : IEquatable<T>
		{
			if (results.TryGetValue(typeof(T), out TestResult result))
			{
				return Validate(original, (T) result.Result);
			}

			return false;
		}
		
		public bool ValidateList<T, U>(T originalList) where T : IList<U>
		{
			if (results.TryGetValue(typeof(T), out TestResult result))
			{
				return ValidateList<T, U>(originalList, (T) result.Result);
			}

			return false;
		}

		public abstract void Cleanup();
			
		protected abstract long Serialize<T>(T original);
		protected abstract T Deserialize<T>();

		protected virtual bool Validate<T>(T original, T copy) where T : IEquatable<T>
		{
			return EqualityComparer<T>.Default.Equals(original, copy);
		}
		
		protected virtual bool ValidateList<T, U>(T originalList, T copyList) where T : IList<U>
		{
			return originalList.SequenceEqual(copyList);
		}
	}
}