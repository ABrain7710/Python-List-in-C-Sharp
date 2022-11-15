using System.Text;
using System.Collections.Generic;
using System.Collections;

namespace DynArrayImplementation
{

    class NotComparableException : Exception
    {
        public override string Message
        {
            get
            {
                return "Type does not implement the IEquatable<T> interface";
            }
        }
        public override string HelpLink
        {
            get
            {
                return "Get More Information from here: https://learn.microsoft.com/en-us/dotnet/api/system.iequatable-1?redirectedfrom=MSDN&view=net-7.0";
            }
        }
    }

    class EmptyArrayException : Exception
    {
        public override string Message
        {
            get
            {
                return "Empty Array";
            }
        }
    }

    public class DynArray<T>
    {

        private T[] _data;
        private int maxUserIndex;

        public DynArray(T[] initalData)
        {

            if (!typeof(IEquatable<T>).IsAssignableFrom(typeof(T)))
            {
                throw new NotComparableException();
            }

            this.maxUserIndex = initalData.Length-1;
            this._data = initalData;
        }

        public DynArray(List<T> initalData)
        {
            if (!typeof(IEquatable<T>).IsAssignableFrom(typeof(T)))
            {
                throw new NotComparableException();
            }

            this.maxUserIndex = initalData.Count - 1;

            this._data = new T[initalData.Count];
            initalData.CopyTo(this._data);
        }

        public DynArray(): this(new T[0])
        {
        }

        private DynArray(T[] initialData, int maxUserIndex){
            this._data = initialData;
            this.maxUserIndex = maxUserIndex;
        }

        public void Append(T item)
        {
            if(item == null){
                return;
            }

            if(this.resizeNeeded()){
                this.ResizeArray();
            }

            this._data[maxUserIndex + 1] = item;
            this.maxUserIndex++;
        }

        public void Prepend(T item)
        {
            if(item == null){
                return;
            }

            if (this.resizeNeeded())
            {
                // resize the array and offset the values to the right 1 so we can prepend the value
                this.ResizeArray(offset: 1);
                
            }
            else {
                for(int i = this.maxUserIndex; i >= 0; i--){
                    this._data[i+1] = this._data[i];
                }
            }

            this._data[0] = item;
            this.maxUserIndex++;
        }

        public void Prepend(T[] items){
            if(items == null){
                return;
            }

            if (this.resizeNeeded(items.Length))
            {
                this.ResizeArray(offset: items.Length, slotsNeeded: items.Length);
            }
            else
            {
                for (int i = this.maxUserIndex; i >= 0; i--)
                {
                    this._data[i + items.Length] = this._data[i];
                }
            }

            for(int i = 0; i < items.Length; i++){
                this._data[i] = items[i];
            }
            this.maxUserIndex += items.Length;
        }

        public void Extend(T[] items){

            if(items == null || items.Length == 0){
                return;
            }

            if (this.resizeNeeded(items.Length))
            {
                // resize the array to fit the array of items
                this.ResizeArray(offset: 0, slotsNeeded: items.Length);
            }

            int j = 0;
            for(int i = this.maxUserIndex + 1; i <= this.maxUserIndex + items.Length; i++){
                this._data[i] = items[j];
                j++;
            }
           this.maxUserIndex += items.Length;
        }

        public void Insert(int index, T item)
        {
            if(item == null){
                return;
            }

            if(index < 0){
                index = convertNegativeIndexToPositive(index);
            }

            if (this.resizeNeeded())
            {
                this.ResizeArray();
            }

            if (index > this.maxUserIndex)
            {
                this._data[this.maxUserIndex+1] = item;
                this.maxUserIndex++;
                return;
            }
           
            for(int i = this.maxUserIndex; i >= index; i--){
                this._data[i+1] = this._data[i];
            }
            
            this._data[index] = item;
            this.maxUserIndex++;
        }

        public void Set(int index, T item)
        {
            if(index < 0){
                index = convertNegativeIndexToPositive(index);
            }

            if(item == null){
                return;
            }
            if(index > this.maxUserIndex){
                throw new IndexOutOfRangeException();
            }

            this._data[index] = item;
        }

        public T Get(int index)
        {
            if(index < 0){
                index = convertNegativeIndexToPositive(index);
            }

            if (index > maxUserIndex)
            {
                throw new IndexOutOfRangeException();
            }
            return this._data[index];
        }

        public int index(T item)
        {
            if(item == null){
                return -1;
            }

            for (int i = 0; i < this._data.Length; i++)
            {
                if(item.Equals(this._data[i])){
                    return i;
                }
            }

            return -1;
        }

        public void Clear(){
            this._data = new T[0];
            this.maxUserIndex = this._data.Length -1;
        }

        public T Pop(){
            if(this.maxUserIndex == -1){
                throw new EmptyArrayException();
            }
            
            T item = this._data[this.maxUserIndex];
            this.maxUserIndex--;
            return item;
        }

        public int GetLength(){
            return this.maxUserIndex + 1;
        }

        public void Remove(T item){
            // removes the first instance of an item
            if (this.maxUserIndex == -1)
            {
                throw new EmptyArrayException();
            }

            int index = this.index(item);
            if(index == -1){
                return;
            }

            this.Delete(index);
        }

        public void Delete(int index){
            if(index > this.maxUserIndex){
                throw new IndexOutOfRangeException();
            }

            for (int i = index; i < this.maxUserIndex; i++)
            {
                this._data[i] = this._data[i + 1];
            }
            this.maxUserIndex--;
        }

        public void Add(T[] items){
            this.Extend(items);
        }

        public int Count(T item){
            if(item == null){
                return 0;
            }

            int count = 0;
            for(int i = 0; i <= this.maxUserIndex; i++){
                if(item.Equals(this._data[i])){
                    count++;
                }
            }
            return count;
        }

        public bool Contains(T item){
            return this.index(item) != -1;
        }

        public void Reverse(){
            Array.Reverse(this._data);
        }

        public DynArray<T> Copy(){
            return new DynArray<T>(this._data, this.maxUserIndex);
        }

        public T[] GetData(){
            
            T[] data = new T[this.maxUserIndex+1];

            Array.Copy(this._data, data, this.maxUserIndex+1);

            return data;
        }

        
        public override string ToString(){
            StringBuilder stringBuilder = new StringBuilder("");
            // stringBuilder.Append("    Length: " + this._data.Length);
            // stringBuilder.Append("    MaxUserIndex: " + this.maxUserIndex);
            stringBuilder.Append("[");
            for(int i = 0; i <= this.maxUserIndex; i++){

                stringBuilder.Append(this._data[i]);
                if(i != this.maxUserIndex){
                    stringBuilder.Append(",");
                }
            }
            stringBuilder.Append("]");

            return stringBuilder.ToString();
        }

        private void ResizeArray(int offset = 0, int slotsNeeded = 1)
        {
            // ensures that there is enough room for the data that is being added
            int numSlotsToAdd = 4;
            while(offset + slotsNeeded > numSlotsToAdd){
                numSlotsToAdd *=2;
            }

            // allocate the new array at the increased size
            T[] newArray = new T[this._data.Length + numSlotsToAdd];

            // copy this._data to newArray
            for (int i = 0; i < this._data.Length; i++)
            {
                newArray[i + offset] = this._data[i];
            }

            this._data = newArray;
        }

        private bool resizeNeeded(int slotsNeeded = 1){

            return this.maxUserIndex >= this._data.Length - slotsNeeded;
        }

        private int convertNegativeIndexToPositive(int index){


            return index + this.maxUserIndex + 1;
        }
    }

    class Program
    {
        static int Main(string[] args)
        {
            // here for running quick tests
            return 0;
        }

    }
}
