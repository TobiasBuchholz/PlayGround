using System;
using System.Collections.Generic;
using System.Reactive;
using PlayGround.Models;

namespace PlayGround.Contracts.Repositories
{
	public interface ICoversRepository
	{
	 	IObservable<IEnumerable<Cover>> GetCovers();
		IObservable<Unit> UpdateCovers();
	}
}
