using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Resources;
using System.Text;

namespace KaorLocale
{
	/// <summary>
	/// Менеджер и18н ресурсов
	/// Выполнен в виде синглтона
	/// При инициализации подгружает нужную локализацию
	/// </summary>
	public class ResMgr
	{
		ResourceManager resourceManager;
		CultureInfo cultureInfo;

		ResMgr(string pLocale)
		{
			resourceManager = new ResourceManager("KaorCore.Locale", Assembly.GetExecutingAssembly());
			cultureInfo = new CultureInfo(pLocale);
		}

		#region Секция синглтона
		static ResMgr instance;
		static ResMgr Instance
		{
			get
			{
				if (instance == null)
				{
					/// Создание инстанса с культурой по-умолчанию
					instance = new ResMgr(CultureInfo.CurrentCulture.Name);
					//throw new InvalidOperationException("Need to initialize instance of ResMgr via CreateInstance(...) method first");
				}
				return instance;
			}
		}

		public static void CreateInstance(string pLocale)
		{
			instance = new ResMgr(pLocale);
		}

		public static string GetString(string pStringName)
		{
			return Instance.resourceManager.GetString(pStringName);
		}

		public static object GetObject(string pObjectName)
		{
			return Instance.resourceManager.GetObject(pObjectName, Instance.cultureInfo);
		}
		#endregion
	}
}
