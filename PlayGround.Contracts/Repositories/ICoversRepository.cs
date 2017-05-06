using System;
using System.Collections.Generic;
using PlayGround.Models;

namespace PlayGround.Contracts.Repositories
{
	public interface ICoversRepository
	{
	 	IObservable<IEnumerable<Cover>> GetCovers();
	}
}
