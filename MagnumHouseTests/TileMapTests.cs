using System;
using System.Collections.Generic;
using NUnit.Framework;
using MagnumHouseLib;

namespace MagnumHouseTests
{
	[TestFixture]
	public class TileMapTests
	{
		int[,] Level1 = new int [,] {
			{0,0,0,0},
			{0,0,0,0},
			{1,0,0,1},
			{1,1,1,1}
		};
		
		IEnumerable<TestData> Level1TestCases = new [] {
			new TestData { //completely clear
				Position = new Vector2f(2f,2f),
				ExpectedProximity = new TileProximity
					{RightClear = 0.0f, LeftClear = 0.0f, UpClear = 0.0f, DownClear = 0.0f}
			},
			new TestData { //bottom collide
				Position = new Vector2f(1.5f,0.8f),
				ExpectedProximity = new TileProximity
					{ Below = 1, On = 1, Left = 1, Right = 1,
					RightClear = 0f, LeftClear = 0f, UpClear = 0.8f, DownClear = 0.2f}
			},
			new TestData { //just clear
				Position = new Vector2f(1.5f,1f),
				ExpectedProximity = new TileProximity
					{RightClear = 0.5f, LeftClear = 0.5f, UpClear = 0.5f, DownClear = 0.5f}
			},
			new TestData { //right collide
				Position = new Vector2f(3.2f,1f),
				ExpectedProximity = new TileProximity { Below = 1, On = 1, Right = 1, Above = 1,
					RightClear = 0.2f, LeftClear = 0.8f, UpClear = 0.5f, DownClear = 0.5f }
			}
		};
		
		int[,] EmptyLevel = new int [,] {
			{0,0,0,0},
			{0,0,0,0},
			{0,0,0,0},
			{0,0,0,0}
		};
		
		IEnumerable<TestData> EmptyLevelTestCases = new [] {
			new TestData {
				Position = new Vector2f(0,0.5f),
				ExpectedProximity = new TileProximity {RightClear = 0.0f, LeftClear = 0.0f, UpClear = 0.5f, DownClear = 0.5f}
			},
			new TestData {
				Position = new Vector2f(0.5f,0.5f),
				ExpectedProximity = new TileProximity {RightClear = 0.5f, LeftClear = 0.5f, UpClear = 0.5f, DownClear = 0.5f}
			},
			new TestData {
				Position = new Vector2f(0.5f,0),
				ExpectedProximity = new TileProximity {RightClear = 0.5f, LeftClear = 0.5f, UpClear = 0.5f, DownClear = 0.5f}
			}
		};
		
		[Test]
		public void CheckProximityInLevel1()
		{
			AssertCorrectProximities(Level1TestCases, new TileMap(Level1));
		}
		
		[Test]
		public void CheckProximityInEmptyLevel()
		{
			AssertCorrectProximities(EmptyLevelTestCases, new TileMap(EmptyLevel));
		}
		
		private void AssertCorrectProximities(IEnumerable<TestData> _expected, TileMap _map) {
			int testCaseIndex = 0;
			foreach (TestData data in _expected) {
				TileProximity returnedProximity = _map.GetProximity(data.Position);
				AssertEqualProximity(data.ExpectedProximity, returnedProximity, testCaseIndex);
				AssertEqualClearance(data.ExpectedProximity, returnedProximity, testCaseIndex);
				testCaseIndex ++;
			}
		}
		
		private void AssertEqualProximity(TileProximity _one, TileProximity _two, int _id) {
			Assert.That(_one.On == _two.On &&
			       _one.Above == _two.Above &&
			       _one.Below == _two.Below &&
			       _one.Right == _two.Right &&
			       _one.Left == _two.Left,
			       String.Format("Proximities not equal, expected:\r\n{0}\r\ngot:\r\n{1} in test case {2}",
			                     _one, _two, _id));
		}
		
		private void AssertEqualClearance(TileProximity _one, TileProximity _two, int _id) {
			Assert.AreEqual(_one.RightClear, _two.RightClear, "clear right not equal in test case " + _id);
			Assert.AreEqual(_one.LeftClear, _two.LeftClear, "clear left not equal in test case " + _id);
			Assert.AreEqual(_one.UpClear, _two.UpClear, "clear up not equal in test case " + _id);
			Assert.AreEqual(_one.DownClear, _two.DownClear, "clear down not equal in test case " + _id);
		}
	}
	
	public class TestData {
		public Vector2f Position;
		public TileProximity ExpectedProximity;
	}
}
