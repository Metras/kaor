// Copyright (c) 2009 CJSC NII STT (http://www.niistt.ru) and the 
// individuals listed on the AUTHORS entries.
// All rights reserved.
//
// Authors: 
//          Valentin Yakovenkov <yakovenkov@niistt.ru>
//
// Redistribution and use in source and binary forms, with or without
// modification, are permitted provided that the following conditions
// are met:
// 1.  Redistributions of source code must retain the above copyright
//     notice, this list of conditions and the following disclaimer.
// 2.  Redistributions in binary form must reproduce the above copyright
//     notice, this list of conditions and the following disclaimer in the
//     documentation and/or other materials provided with the distribution.
// 3.  Neither the name of CJSC NII STT ("NII STT") nor the names of
//     its contributors may be used to endorse or promote products derived
//     from this software without specific prior written permission.
//
// THIS SOFTWARE IS PROVIDED BY NII STT AND ITS CONTRIBUTORS "AS IS" AND
// ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE
// IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE
// ARE DISCLAIMED. IN NO EVENT SHALL NII STT OR ITS CONTRIBUTORS BE LIABLE FOR
// ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL
// DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS
// OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION)
// HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT,
// STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING
// IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE
// POSSIBILITY OF SUCH DAMAGE.
//

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Xml;

using KaorCore.RPU;

namespace RPUICOMR8500.PowerMeter
{
    class R8500PowerMeter : IPowerMeter
    {
        //одна строчка таблицы
        struct TableItem
        {
            public int raw; //показания приемника
            public float val;  //значение в децибеллах
            
            public TableItem(int praw, float pval)
            {
                raw = praw;
                val = pval;
            }
        }
        //одна калибровочная таблица для определенного диапазона частот
        struct Table
        {
            public double fMin, fMax; //мин макс частота
            public TableItem[] items; //сама таблица

            public Table(double pfmin, double pfmax, TableItem[] pItems)
            {
                fMin = pfmin;
                fMax = pfmax;
                items = pItems;
            }
        }

		bool isPowerMeterRunning = false;
		Thread measureThread;

        /// <summary>
        /// создает новый экземпляр класса
        /// </summary>
        /// <param name="pRPU">родительский приемник</param>
        public R8500PowerMeter(RPUR8500 pRPU)
        {
            locking = new object();
            baseRPU = pRPU;
            averageTime = 1;
            defaultTables = new List<Table>();
            tables = new List<Table>();
            TableItem[] _its;

            //калибровочные таблицы по умолчанию
            _its = new TableItem[22]
            {
            new TableItem(0, -115),
            new TableItem(41, -110),	 
		    new TableItem(59, -105), 
		    new TableItem(71, -100), 
		    new TableItem(82, -95), 
		    new TableItem(98, -90), 
		    new TableItem(112, -85), 
		    new TableItem(129, -80), 
		    new TableItem(141, -75), 
		    new TableItem(151, -70), 
		    new TableItem(158, -65),  
		    new TableItem(167, -60), 
		    new TableItem(174, -55), 
		    new TableItem(183, -50), 
		    new TableItem(190, -45), 
		    new TableItem(199, -40), 
		    new TableItem(206, -35),
            new TableItem(213, -30), 
		    new TableItem(219, -25),
            new TableItem(225, -20),
            new TableItem(231, -15),
            new TableItem(236, -10),
            };            
            defaultTables.Add(new Table(0.1, 30, _its));

            _its = new TableItem[22]
            {
            new TableItem(0, -110),	 
		    new TableItem(49, -105), 
		    new TableItem(64, -100), 
		    new TableItem(78, -95), 
		    new TableItem(93, -90), 
		    new TableItem(109, -85), 
		    new TableItem(123, -80), 
		    new TableItem(138, -75), 
		    new TableItem(149, -70), 
		    new TableItem(158, -65),  
		    new TableItem(165, -60), 
		    new TableItem(172, -55), 
		    new TableItem(176, -50), 
		    new TableItem(183, -45), 
		    new TableItem(192, -40), 
		    new TableItem(202, -35),
            new TableItem(214, -30), 
		    new TableItem(223, -27),
            new TableItem(226, -25),
            new TableItem(236, -20),
            new TableItem(243, -15),
            new TableItem(247, -10),
            };
            defaultTables.Add(new Table(30, 250, _its));
            

            _its = new TableItem[22]
            {
            new TableItem(0, -110),	 
		    new TableItem(49, -105), 
		    new TableItem(64, -100), 
		    new TableItem(78, -95), 
		    new TableItem(93, -90), 
		    new TableItem(109, -85), 
		    new TableItem(123, -80), 
		    new TableItem(138, -75), 
		    new TableItem(149, -70), 
		    new TableItem(158, -65),  
		    new TableItem(165, -60), 
		    new TableItem(172, -55), 
		    new TableItem(183, -50), 
		    new TableItem(190, -45), 
		    new TableItem(199, -40), 
		    new TableItem(206, -35),
            new TableItem(214, -30), 
		    new TableItem(223, -27),
            new TableItem(222, -25),
            new TableItem(229, -20), 
            new TableItem(238, -15),
            new TableItem(243, -10), 
            };            
            defaultTables.Add(new Table(250, 1000, _its));

            _its = new TableItem[24]
            {
            new TableItem(0, -120),	 
		    new TableItem(22, -115),
            new TableItem(44, -110),	 
		    new TableItem(55, -105), 
		    new TableItem(66, -100), 
		    new TableItem(76, -95), 
		    new TableItem(90, -90), 
		    new TableItem(104, -85), 
		    new TableItem(117, -80), 
		    new TableItem(132, -75), 
		    new TableItem(144, -70), 
		    new TableItem(152, -65),  
		    new TableItem(160, -60), 
		    new TableItem(167, -55), 
		    new TableItem(176, -50), 
		    new TableItem(183, -45), 
		    new TableItem(192, -40), 
		    new TableItem(199, -35),
            new TableItem(208, -30), 
		    new TableItem(213, -27),
            new TableItem(215, -25),
            new TableItem(223, -20), 
            new TableItem(231, -15),
            new TableItem(236, -10), 
            };
            defaultTables.Add(new Table(1000, 2000, _its));
        }        

        #region ========Поля========

        RPUR8500 baseRPU;  //приемник
        object locking;    //объект для lock'a 
        bool isRunning;    //запущен ли процесс измерения мощности    

        /// <summary>
        /// таблицы для перевода мощности в децибеллы
        /// </summary>
        List<Table> defaultTables, tables;

        #endregion

        /// <summary>
        /// загружает калибровочную таблицу из XML файла
        /// </summary>
        /// <param name="pNode"></param>
        public void LoadCalibrationFromXML(XmlNode pNode)
        {
            XmlNode _node = pNode.SelectSingleNode("Tables"); //таблицы
            foreach (XmlNode _txn in _node.ChildNodes)  
            {
                if (_txn.Name == "Table")  //если это таблица
                {
                    Table _tb = new Table();
                    List<TableItem> _items = new List<TableItem>();

                    //считываем минимум и максимум
                    //эксепшн при неудаче
                    if (_txn.Attributes["Fmin"] != null && _txn.Attributes["Fmax"] != null)
                    {
						if (!double.TryParse(_txn.Attributes["Fmin"].Value, NumberStyles.Float, CultureInfo.InvariantCulture, out _tb.fMin))
                            throw new ArgumentException("Error parsing fmin");
						if (!double.TryParse(_txn.Attributes["Fmax"].Value, NumberStyles.Float, CultureInfo.InvariantCulture, out _tb.fMax))
                            throw new ArgumentException("Error parsing fmax");
                    }
                    foreach (XmlNode _ixn in _txn.ChildNodes)  //просматривам все строчки таблицы
                    {
                        if (_ixn.Name == "TableItem")
                        {
                            try  //если неудача просто строчку не добавляем
                            {
                                int _rig;
                                float _val;                                
                                _rig = int.Parse(_ixn.Attributes["rig"].Value);
                                _val = float.Parse(_ixn.Attributes["val"].Value);
                                TableItem _item = new TableItem(_rig, _val);

                                //строчки должны быть упорядочены по возрастанию
                                if (_items.Count == 0)
                                    _items.Add(_item);
                                else if (_item.raw > _items[_items.Count - 1].raw) 
                                    _items.Add(_item);                          
                            }
                            catch { }
                        }
                    }
                    _tb.items = _items.ToArray();
                    tables.Add(_tb);
                }
            }
        }

        #region IPowerMeter Members

        int filterBand;
        public int FilterBand
        {
            get
            {
                return filterBand;
            }
            set
            {
				if (value == 2200 ||
					value == 5500 ||
					value == 12000 ||
					value == 150000)
				{
					filterBand = value;

					switch (filterBand)
					{
						case 2200:
							{
								byte[] buffer = { 0xFE, 0xFE, 0x4A, 0xE0, 0x06, 0x03, 0x01, 0xFD };
								byte[] answer = baseRPU.SendCommand(buffer);
							}
							break;

						case 5500:
							{
								byte[] buffer = { 0xFE, 0xFE, 0x4A, 0xE0, 0x06, 0x02, 0x02, 0xFD };
								byte[] answer = baseRPU.SendCommand(buffer);
							}
							break;

						case 12000:
							{
								byte[] buffer = { 0xFE, 0xFE, 0x4A, 0xE0, 0x06, 0x05, 0x01, 0xFD };
								byte[] answer = baseRPU.SendCommand(buffer);
							}
							break;

						case 150000:
							if (baseRPU.BaseFreq < 30000000)
							{
								byte[] buffer = { 0xFE, 0xFE, 0x4A, 0xE0, 0x06, 0x05, 0x01, 0xFD };
								byte[] answer = baseRPU.SendCommand(buffer);
							}
							else
							{
								byte[] buffer = { 0xFE, 0xFE, 0x4A, 0xE0, 0x06, 0x06, 0x01, 0xFD };
								byte[] answer = baseRPU.SendCommand(buffer);
							}
							break;

						default:
							{
								byte[] buffer = { 0xFE, 0xFE, 0x4A, 0xE0, 0x06, 0x05, 0x01, 0xFD };
								byte[] answer = baseRPU.SendCommand(buffer);
							}
							break;
					}
				}
            }
        }

        int averageTime;
        /// <summary>
        /// количество измерений для усреднения
        /// (чем больше тем более усредненное значение возвращается 
        /// при постоянном измерении мощности)
        /// </summary>
        public int AverageTime
        {
            get
            {
                return averageTime;
            }
            set
            {
                if (value < 1)
                    averageTime = 1;
                else
                    averageTime = value;
            }
        }

        /// <summary>
        /// перевод полученной от приемника мощности в дБ
        /// с использованем калибровочной таблицы
        /// </summary>
        /// <param name="pRawval"></param>
        /// <returns></returns>
        public double rig_raw2val(int pRawval)
        {
            double _interpolation;
            int _i;
            Table _table;

            if (tables == null || tables.Count == 0)  //если XML таблицы не загружены
            {
                if (defaultTables == null || defaultTables.Count == 0) //если и таблиц по умолчанию нет
                    return 0;

                _table = defaultTables[0];   //если ничего не найдем подходящего берем первую

                foreach (Table tb in defaultTables) //ищем подходящую по диапазону таблицу
                {
                    if (baseRPU.BaseFreq >= tb.fMin && baseRPU.BaseFreq <= tb.fMax) //нашли
                    {
                        _table = tb;  //берем
                        break;   //дальше не ищем
                    }
                }
            }
            else  //если загружены XML таблицы 
            {

				_table = tables[0];

                var q = from _t in tables
                        where _t.fMin <= baseRPU.BaseFreq &&
                        _t.fMax >= baseRPU.BaseFreq
                        select _t;

				if (q.Count() >= 1)
					_table = q.First();

                //foreach (Table tb in tables)
                //{
                //    if (baseRPU.BaseFreq >= tb.fMin && baseRPU.BaseFreq <= tb.fMax)
                //    {
                //        _table = tb;
                //        break;
                //    }
                //}
            }            

            if (_table.items.Length == 0) //таблица пустая
                return 0;

            for (_i = 0; _i < _table.items.Length; _i++)
                if (pRawval < _table.items[_i].raw)     //ищем между какими значениями в таблице
                    break;                              //находится наше

            if (_i == 0)        //полученное значени <= самому маленькому в таблице
                return _table.items[0].val;

            if (_i >= _table.items.Length)      //полученное значени >= самому большому в таблице
                return _table.items[_i - 1].val;

            if (_table.items[_i].raw == _table.items[_i - 1].raw)	/* catch divide by 0 error */
                return _table.items[_i].val;


            _interpolation = ((_table.items[_i].raw - pRawval) *        //считаем интерполяцию
                     (double)(_table.items[_i].val - _table.items[_i - 1].val)) /
                      (double)(_table.items[_i].raw - _table.items[_i - 1].raw);
            return _table.items[_i].val - _interpolation;
        }

        /// <summary>
        /// измерить мощность на текущей частоте
        /// </summary>
        /// <returns></returns>
        public double MeasurePower()
        {
            lock (locking)
            {
				int _powerRate = 0;

                //отправляем команду, получаем ответ
                byte[] _buffer = { 0xFE, 0xFE, 0x4A, 0xE0, 0x15, 0x02, 0xFD };
                byte[] _ans = baseRPU.SendCommand(_buffer);
                //если ответ на данную команду
                //(измерить мощность)
				if (_ans == null)
					return -140.0;

				if(_ans[2] == 0x15 && _ans[3] == 0x02)
				{
					_powerRate = (int)((double)baseRPU.BCDtoInt(_ans, 4) * 1.0);
				}
				return rig_raw2val((int)(_powerRate)); //преобразуем в дБ и возвращам
            }
        }
        /// <summary>
        /// измерить мощность на заданной частоте
        /// </summary>
        /// <param name="pBaseFreq">новая частота</param>
        /// <returns></returns>
        public double MeasurePower(long pBaseFreq)
        {
			baseRPU.BaseFreq = pBaseFreq;   //переустанавливаем частоту
			return MeasurePower();   //измеряем

        }
        /// <summary>
        /// идет ли процесс измерения мощности
        /// </summary>
        public bool IsRunning
        {
            get { return isRunning; }
        }
        /// <summary>
        /// начать измерение мощности
        /// </summary>
		/// 
#if false
        public void Start()
        {
            if (!isRunning)
            {
                isRunning = true;
                ThreadStart _tds = new ThreadStart(Measuring);
                Thread _measureThread = new Thread(_tds);  //запускаем измерение в новом потоке
                _measureThread.Start();
            }
        }
#else
		public void StartThread()
		{
			if (!isPowerMeterRunning)
			{
				isPowerMeterRunning = true;
				//ThreadStart _tds = new ThreadStart(Measuring);
				measureThread = new Thread(Measuring);  //запускаем измерение в новом потоке
				measureThread.Start();
			}
		}

		public void StopThread()
		{
			isPowerMeterRunning = false;

			measureThread.Abort();
			//measureThread.Join();
			
		}

		public void Start()
		{
			isRunning = true;
		}

#endif
        /// <summary>
        /// остановить измерения мощности
        /// </summary>
        public void Stop()
        {
            isRunning = false;
        }

        /// <summary>
        /// непрерывно измеряет мощность
        /// </summary>
		/// 
#if false
        void Measuring()
        {
            while (isRunning)
            {
                double _average = 0;  //среднее значение мощности за АverageTime измерений
                for (int _i = 0; _i < averageTime; _i++)
                {
                    double _d = MeasurePower();         //измеряем
                    _average = (_d + _average * _i)  / (_i + 1);  //рассчет среднего значения                  
                }
                RaiseOnNewPowerMeasure(baseRPU, baseRPU.BaseFreq, (float)_average);
            }
        }
#else
		void Measuring()
		{

			try
			{

				while (isPowerMeterRunning)
				{
					double _average = 0;  //среднее значение мощности за АverageTime измерений
					for (int _i = 0; _i < averageTime; _i++)
					{
						double _d = MeasurePower();         //измеряем
						_average = (_d + _average * _i) / (_i + 1);  //рассчет среднего значения                  
					}
					RaiseOnNewPowerMeasure(baseRPU, baseRPU.BaseFreq, (float)_average);

					if (!isRunning)
						Thread.Sleep(100);
				}
			}

			catch { }
		}
#endif
#if false
        /// <summary>
        /// Создает новую трассу используя диалог
        /// </summary>
        /// <returns></returns>
        public KaorCore.Trace.BaseTrace UserCreateNewTrace(long pFstart, long pFstop)
        {
            R8500Trace _trace = null;  //если не ОК, вернем null

            NewR8500TraceDialog _dlg = new NewR8500TraceDialog();
            _dlg.txtFstart.Text = pFstart.ToString(CultureInfo.InvariantCulture);    //начальная и конечная частота
            _dlg.txtFstop.Text = pFstop.ToString(CultureInfo.InvariantCulture);      //изначально вписанная в поле ввода  

            _dlg.Antennas = baseRPU.Antennas; //список антенн

            if (_dlg.ShowDialog() == DialogResult.OK)
            {                
                _trace = new R8500Trace(_dlg.FStart, _dlg.FStop,
                    _dlg.FilterBand, _dlg.FilterBand, 0,
                    baseRPU, _dlg.antenna);         //создаем экземпляр трассы
                //свойства трассы
                _trace.Name = _dlg.txtTraceName.Text; //имя
                _trace.Description = _dlg.txtTraceDescr.Text;    //описание
                _trace.LineItem.Color = _dlg.cmbTraceColor.Value; //цвет
                _trace.TaskProvider.CycleMode = _dlg.chkCycle.Checked;  //зациклена ли
            }
            
            return _trace;
        }

        /// <summary>
        /// не задано
        /// </summary>
        /// <param name="pFstart"></param>
        /// <param name="pFstop"></param>
        /// <returns></returns>
        public KaorCore.Trace.BaseTrace UserQuickCreateNewTrace(long pFstart, long pFstop)
        {
            throw new NotImplementedException();
        }
#endif
        /// <summary>
        /// Измерена мощность
        /// </summary>
        public event NewPowerMeasure OnNewPowerMeasure;

       
        /// <summary>
        /// вызывает событие OnNewPowerMeasure
        /// </summary>
        /// <param name="pRPU">приемник</param>
        /// <param name="pFrequency">частота измерения</param>
        /// <param name="pPower">мощность</param>
        void RaiseOnNewPowerMeasure(IRPU pRPU, long pFrequency, float pPower)
        {
            if (OnNewPowerMeasure != null)
            {
                OnNewPowerMeasure(pRPU, pFrequency, pPower);
            }
        }

        #endregion
    }
}
