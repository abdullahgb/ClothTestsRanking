using System;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Content;
using System.Collections.Generic;
using Shopping.DemoApp.Models;
using System.Linq;
using System.Globalization;
using Android.Widget;
using Android.OS;
using System.Threading.Tasks;
using Android;
using Java.IO;

namespace SemesterProject
{
    public class SaleRecyclerAdapter : RecyclerView.Adapter, View.IOnClickListener
    {
        private readonly List<SaleItem> saleItems;

        private readonly Context context;

        private readonly Action<View, int> itemClickListener;
        private readonly Action<View, int> buttonClickListener;

        public SaleRecyclerAdapter(Context context,
            List<SaleItem> saleItems,
            Action<View, int> itemClickListener,
            Action<View, int> buttonClickListener)
        {
            this.context = context;
            this.saleItems = saleItems;
            this.itemClickListener = itemClickListener;
            this.buttonClickListener = buttonClickListener;
        }

        public override int GetItemViewType(int position)
        {
            var dummyItemPosition = this.saleItems.Count <= 1 ? 0 : 1;

            var viewType = SaleItemViewType.SaleItem;
            if (position == dummyItemPosition)
            {
                viewType = SaleItemViewType.SellButton;
            }

            return (int)viewType;
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {

            var view = default(View);
            var viewHolder = default(RecyclerView.ViewHolder);

            var layoutInflater = LayoutInflater.From(parent.Context);

            var saleItemViewType = (SaleItemViewType)viewType;
            if (saleItemViewType == SaleItemViewType.SaleItem)
            {
                view = layoutInflater.Inflate(Resource.Layout.saleitemviewcell, parent, false);
                viewHolder = new SaleItemViewHolder(view);
            }
            else if (saleItemViewType == SaleItemViewType.SellButton)
            {
                view = layoutInflater.Inflate(Resource.Layout.buttonitemviewcell, parent, false);
                viewHolder = new SellItemViewHolder(view);
            }

            view.Tag = viewHolder;
            view.SetOnClickListener(this);

            return viewHolder;
        }

        public override async void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            var item = saleItems.ElementAt(position);
            var viewHolder = holder as SaleItemViewHolder;

            if (viewHolder != null && item != null)
            {
                viewHolder.DisplayName.Text = item.Name.ToUpperInvariant();
                viewHolder.Description.Text = item.Description;

                var priceRounded = Math.Round(item.Price);
                viewHolder.Price.Text = priceRounded.ToString(CultureInfo.InvariantCulture);
                viewHolder.PriceCurrency.Text = ""; //NumberFormat.CurrencySymbol
                //viewHolder.Thumbnail.SetImageResource(new Uri().LocalPath);
                // await BindImage(item, viewHolder.Thumbnail);
            }
        }


        public override int ItemCount
        {
            get { return this.saleItems.Count(); }
        }

        public void OnClick(View v)
        {
            var saleViewHolder = v.Tag as SaleItemViewHolder;
            if (saleViewHolder != null)
            {
                itemClickListener?.Invoke(v, saleViewHolder.AdapterPosition);
            }

            //var sellViewHolder = v.Tag as SellItemViewHolder;
            //if (sellViewHolder != null)
            //{
            //    buttonClickListener?.Invoke(v, sellViewHolder.AdapterPosition);
            //}
        }
    }
}