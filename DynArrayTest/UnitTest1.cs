using System.Linq;
using DynArrayImplementation;
using System.Collections;

namespace DynArrayTest;

[TestClass]
public class DynArrayUnitTest
{    

    [TestMethod]
    public void TestDynArrayListConstructor()
    {

        int[] startData = { 1, 8, 4, 3 };

        // test dyn array initialization with a list
        List<int> testList = new List<int>(startData);
        DynArray<int> array = new DynArray<int>(testList);
        int[] arrayData = array.GetData();

        Assert.IsTrue(this.SameArray(arrayData, testList.ToArray()));
    }

    [TestMethod]
    public void TestDynArrayStaticArrayConstructor()
    {

        int[] startData = { 1, 8, 4, 3 };

        // test dyn array initialization with a static array
        DynArray<int> array = new DynArray<int>(startData);
        int[] arrayData = array.GetData();

        Assert.IsTrue(this.SameArray(arrayData, startData));
    }

    [TestMethod]
    public void TestDynArrayNoDataConstructor()
    {

        // test dyn array initialization with a static array
        DynArray<int> array = new DynArray<int>();
        int[] arrayData = array.GetData();

        Assert.IsTrue(this.SameArray(arrayData, new int[0]));
    }

    [TestMethod]
    public void TestDynArrayAppend()
    {
        int[] startData = { 1, 8, 4, 3 };

        // test dyn array initialization with a static array
        DynArray<int> array = new DynArray<int>();

        foreach(int item in startData){
            array.Append(item);
        }

        int[] arrayData = array.GetData();

        Assert.IsTrue(this.SameArray(arrayData, startData));
    }


    [TestMethod]
    public void TestDynArrayPrepend()
    {

        int[] startData = { 1, 8, 4, 3 };

        // test dyn array initialization with a static array
        DynArray<int> array = new DynArray<int>();

        foreach (int item in startData)
        {
            array.Prepend(item);
        }

        int[] arrayData = array.GetData();

        // array needs to be reversed for comparision because we prepended the data
        Array.Reverse(startData);

        Assert.IsTrue(this.SameArray(arrayData, startData));
    }


    [TestMethod]
    public void TestDynArrayExtend()
    {

        int[] arr1 = { 1, 8, 4, 3 };
        int[] arr2 = { 0, 1, 2, 3 };

        // combine arr1 and arr2
        int[] combined = new int[arr1.Length + arr2.Length];
        Array.Copy(arr1, combined, arr1.Length);
        Array.Copy(arr2, 0, combined, arr1.Length, arr2.Length); 

        // initalize dyn array with arr1
        DynArray<int> array = new DynArray<int>(arr1);

        // extend the dyn array with arr2
        array.Extend(arr2);

        // now the dyn array and the combined array should be the same
        int[] arrayData = array.GetData();
        Assert.IsTrue(this.SameArray(arrayData, combined));
    }

    [TestMethod]
    public void TestDynArrayInsert()
    {

        int[] arr1 = { 1, 8, 4, 3 };

        // initalize dyn array with arr1
        DynArray<int> array = new DynArray<int>(arr1);

        // extend the dyn array with arr2
        array.Insert(index: 0, item: 100);
        array.Insert(index: 1000, item: 99);
        array.Insert(index: 2, item: 49);

        int[] finalArray = {100, 1, 49, 8, 4, 3, 99};

        // now the dyn array and the combined array should be the same
        int[] arrayData = array.GetData();
        Assert.IsTrue(this.SameArray(arrayData, finalArray));
    }

    [TestMethod]
    public void TestDynArraySet()
    {

        int[] arr1 = { 1, 8, 4, 3 };

        // initalize dyn array with arr1
        DynArray<int> array = new DynArray<int>(arr1);

        int index1 = 0;
        int value1 = 100;
        int index2 = 2;
        int value2 = 99;

        // extend the dyn array with arr2
        array.Set(index: index1, item: value1);
        array.Set(index: index2, item: value2);

        arr1[index1] = value1;
        arr1[index2] = value2;


        // now the dyn array and the combined array should be the same
        int[] arrayData = array.GetData();
        Assert.IsTrue(this.SameArray(arrayData, arr1));
    }


    [TestMethod]
    public void TestDynArrayGet()
    {

        int[] arr1 = { 1, 8, 4, 3 };

        // initalize dyn array with arr1
        DynArray<int> array = new DynArray<int>(arr1);

        int index1 = 0;
        int index2 = 2;
        int index3 = -1;

        // extend the dyn array with arr2
        Assert.AreEqual(array.Get(index: index1), arr1[index1]);
        Assert.AreEqual(array.Get(index: index2), arr1[index2]);
        Assert.AreEqual(array.Get(index: index3), arr1[index3+arr1.Length]);
    }


    [TestMethod]
    public void TestDynArrayIndex()
    {

        int[] arr1 = { 1, 8, 4, 3 };

        // initalize dyn array with arr1
        DynArray<int> array = new DynArray<int>(arr1);

        int index = 3;
        int value = arr1[index];

        // extend the dyn array with arr2
        Assert.AreEqual(array.index(value), index);
    }

    [TestMethod]
    public void TestDynArrayClear()
    {

        int[] arr1 = { 1, 8, 4, 3 };

        // initalize dyn array with arr1
        DynArray<int> array = new DynArray<int>(arr1);

        array.Clear();

        int[] arrayData = array.GetData();
        Assert.AreEqual(arrayData.Length, 0);
    }

    [TestMethod]
    public void TestDynArrayPop()
    {

        int[] arr1 = { 1, 8, 4, 3 };

        // initalize dyn array with arr1
        DynArray<int> array = new DynArray<int>(arr1);

        int item = array.Pop();
        Assert.AreEqual(item, arr1[arr1.Length-1]);

        int[] arrayData = array.GetData();
        Assert.AreEqual(arrayData.Length, arr1.Length-1);
    }


    [TestMethod]
    public void TestDynArrayGetLength()
    {

        int[] arr1 = { 1, 8, 4, 3 };

        // initalize dyn array with arr1
        DynArray<int> array = new DynArray<int>(arr1);

        Assert.AreEqual(array.GetLength(), arr1.Length);
    }

    [TestMethod]
    public void TestDynArrayDelete()
    {

        int[] arr1 = { 1, 8, 8, 4, 3 };
        int[] finalArray = { 1, 8, 4, 3 };
        int indexToRemove = 1;

        // initalize dyn array with arr1
        DynArray<int> array = new DynArray<int>(arr1);

        array.Delete(indexToRemove);

        int[] arrayData = array.GetData();
        Assert.IsTrue(this.SameArray(arrayData, finalArray));
    }

    [TestMethod]
    public void TestDynArrayRemove()
    {

        int[] arr1 = { 1, 8, 8, 4, 3};
        int[] finalArray = {1, 8, 4, 3};
        int valueToRemove = 8;

        // initalize dyn array with arr1
        DynArray<int> array = new DynArray<int>(arr1);

        array.Remove(valueToRemove);

        int[] arrayData = array.GetData();

        Assert.IsTrue(this.SameArray(arrayData, finalArray));
    }

    [TestMethod]
    public void TestDynArrayCount()
    {

        int[] arr1 = { 1, 8, 8, 4, 8, 10, 13, 8, 7, 10, 4, 7, 3 };
        int valueToCount= 8;

        // initalize dyn array with arr1
        DynArray<int> array = new DynArray<int>(arr1);

        Assert.AreEqual(array.Count(valueToCount), 4);
    }


    [TestMethod]
    public void TestDynArrayContains()
    {

        int[] arr1 = { 1, 8, 8, 4, 8, 10, 13, 8, 7, 10, 4, 7, 3 };

        // initalize dyn array with arr1
        DynArray<int> array = new DynArray<int>(arr1);

        Assert.IsTrue(array.Contains(13));
        Assert.IsFalse(array.Contains(2));
    }

    [TestMethod]
    public void TestDynArrayReverse()
    {

        int[] arr1 = { 0, 1, 2, 3};

        // initalize dyn array with arr1
        DynArray<int> array = new DynArray<int>(arr1);

        array.Reverse();

        Array.Reverse(arr1);


        int[] arrayData = array.GetData();
        Assert.IsTrue(this.SameArray(arrayData, arr1));
    }



    private bool SameArray(int[] arr1, int[] arr2){

        if(arr1.Length != arr2.Length){
            return false;
        }

        for(int i = 0; i < arr1.Length; i++){
            Console.WriteLine(arr1[i] + " vs " + arr2[i]);
            if (arr1[i] != arr2[i])
            {
                return false;
            }
        }
        return true;
    }
}