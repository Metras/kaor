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
// THIS SOFTWARE IS PROVIDED BY APPLE AND ITS CONTRIBUTORS "AS IS" AND
// ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE
// IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE
// ARE DISCLAIMED. IN NO EVENT SHALL APPLE OR ITS CONTRIBUTORS BE LIABLE FOR
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
using System.Linq;
using System.Text;

namespace KaorCore.Undo
{
	public class UndoStack<T>
	{
		Stack<UndoState<T>> undo;
		Stack<UndoState<T>> redo;
		UndoState<T> origState;
		UndoState<T> currentState;

		public UndoStack(T pObject)
		{
			undo = new Stack<UndoState<T>>();
			redo = new Stack<UndoState<T>>();

			origState = new UndoState<T>(pObject);
			currentState = new UndoState<T>(pObject);
		}

		public void Reset()
		{
			currentState = origState;
			undo.Clear();
			redo.Clear();
		}

		public void NewState(T pObject)
		{
			undo.Push(currentState);
			currentState = new UndoState<T>(pObject);

			/// Очистка стека реду при добавлении новой операции
			redo.Clear();
		}

		public T Undo()
		{
			if (undo.Count == 0)
				return currentState.LoadObject();

			redo.Push(currentState);
			currentState = undo.Pop();
			

			return currentState.LoadObject();
		}

		public T Redo()
		{
			if (redo.Count == 0)
				return currentState.LoadObject();

			undo.Push(currentState);
			currentState = redo.Pop();
			

			return currentState.LoadObject();
		}

		public T UndoAll()
		{
			foreach (UndoState<T> _state in undo)
			{
				redo.Push(_state);
			}

			UndoState<T> _lastState = redo.Pop();

			return _lastState.LoadObject();
		}

		public T RedoAll()
		{
			foreach (UndoState<T> _state in redo)
			{
				undo.Push(_state);
			}

			UndoState<T> _lastState = undo.Pop();

			return _lastState.LoadObject();

		}

		public T OriginalObject
		{
			get
			{
				return origState.LoadObject();
			}
		}

		public T CurrentSignal
		{
			get
			{
				return currentState.LoadObject();
			}
		}
	}
}
