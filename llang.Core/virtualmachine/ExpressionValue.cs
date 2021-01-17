using System;
using llang.Core.parser;
using System.Collections.Generic;

namespace llang.Core.virtualmachine
{
	public enum ExpressionValueType
	{
		OBJECT,
		FUNCTION,
		STRING,
		NUMBER,
		BOOLEAN
	}

	public class ExpressionValue
	{
		private ExpressionValueType type;
		private float nValue = 0;
		private bool bValue = false;
		private string sValue = "";
		private Function fValue = new Function("", new Statements(), new List<string>(), null);
		private Dictionary<string, ExpressionValue> members;


		public bool IsBool => this.type == ExpressionValueType.BOOLEAN;
		public bool IsNumber => this.type == ExpressionValueType.NUMBER;
		public bool IsString => this.type == ExpressionValueType.STRING;
		public bool IsFunction => this.type == ExpressionValueType.FUNCTION;
		public bool IsObject => this.type == ExpressionValueType.OBJECT;
		public bool Bool => this.bValue;
		public float Number => this.nValue;

		public string String => this.sValue;

		public Function Function => this.fValue;

		public ExpressionValue (ExpressionValueType type)
		{
			this.type = type;
			if (type == ExpressionValueType.OBJECT)
				this.members = new Dictionary<string, ExpressionValue>();
		}

		public ExpressionValue (ExpressionValueType type, string value) : this(type)
		{
			this.sValue = value;
			this.bValue = (value != "");
			this.nValue = (value != "") ? 1 : 0;
		}

		public ExpressionValue (ExpressionValueType type, float value) : this(type)
		{
			if (float.IsInfinity (value)) {
				this.sValue = "Infinity";
			} else if (float.IsNaN (value)) {
				this.sValue = "NaN";
			} else {
				this.sValue = value.ToString ();
			}

			this.bValue = (value != 0) ? true : false;
			this.nValue = value;
		}

		public ExpressionValue (ExpressionValueType type, bool value) : this(type)
		{
			this.sValue = value.ToString ();
			this.bValue = value;
			this.nValue = (value) ? 1 : 0;
		}

		public ExpressionValue (ExpressionValueType type, Function value) : this(type)
		{
			this.sValue = "function";
			this.bValue = true;
			this.nValue = 1;
			this.fValue = value;
		}

		public void Substitute (ExpressionValue value)
		{
			this.bValue = value.Bool;
			this.nValue = value.Number;
			this.sValue = value.String;
		}
		
		public ExpressionValue GetProperty (string str)
		{
			return this.members [str];
		}

		public void SetProperty (string str, ExpressionValue value)
		{
			this.members.Remove (str);
			this.members.Add (str, value);
		}
	}
	
}