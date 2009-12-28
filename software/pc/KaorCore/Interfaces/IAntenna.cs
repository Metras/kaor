using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KaorCore.Interfaces
{
	public enum EAntennaType
	{
		Directional,
		Omni
	}

	public interface IAntenna
	{
		string Name { get; }
		string Description { get; }

		/// <summary>
		/// Азимутальное направление
		/// </summary>
		double Azimuth { get; }

		/// <summary>
		/// Ширина диаграммы направленности
		/// </summary>
		double DNWidth { get; }

		/// <summary>
		/// Тип антенна
		/// </summary>
		EAntennaType Type { get; }

		/// <summary>
		/// Возможность использования антенны
		/// </summary>
		bool IsEnabled { get; }

		/// <summary>
		/// Частотные диапазон антенны
		/// </summary>
		double FrequencyMin { get; }
		double FrequencyMax { get; }
	}
}
