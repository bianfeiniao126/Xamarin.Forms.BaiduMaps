﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;

using Foundation;

using BMapMain;

namespace Xamarin.Forms.BaiduMaps.iOS
{
    internal class PinImpl : BaseItemImpl<Pin, BMKMapView, BMKPointAnnotation>
    {
        protected override IList<Pin> GetItems(Map map) => map.Pins;

        protected override BMKPointAnnotation CreateNativeItem(Pin item)
        {
            BMKPointAnnotation annotation = new BMKPointAnnotation {
                Title = item.Title,
                //Subtitle = item.Subtitle,
                Coordinate = item.Coordinate.ToNative()
            };

            item.NativeObject = annotation;
            NativeMap.AddAnnotation(annotation);
  
            return annotation;
        }

        protected override void UpdateNativeItem(Pin item)
        {
            BMKPointAnnotation native = (BMKPointAnnotation)item?.NativeObject;
            if (null == native) {
                return;
            }

            item.SetValueSilent(Pin.CoordinateProperty, native.Coordinate.ToUnity());
        }

        protected override void RemoveNativeItem(Pin item)
        {
            NativeMap.RemoveAnnotation((NSObject)item.NativeObject);
        }

        protected override void RemoveNativeItems(IList<Pin> items)
        {
            NSObject[] list = new NSObject[items.Count];
            for (int i = 0; i < items.Count; i++) {
                list[i] = (NSObject)items[i].NativeObject;
            }

            NativeMap.RemoveAnnotations(list);
        }

        internal override void OnMapPropertyChanged(PropertyChangedEventArgs e)
        {
            //throw new NotImplementedException();
        }

        protected override void OnItemPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            Pin item = (Pin)sender;
            BMKPointAnnotation native = (BMKPointAnnotation)item?.NativeObject;
            if (null == native) {
                return;
            }

            if (Annotation.CoordinateProperty.PropertyName == e.PropertyName) {
                native.Coordinate = item.Coordinate.ToNative();
                return;
            }

            if (Annotation.TitleProperty.PropertyName == e.PropertyName) {
                native.Title = item.Title;
                return;
            }

            if (Pin.ImageProperty.PropertyName == e.PropertyName) {
                BMKPinAnnotationView view = (BMKPinAnnotationView)NativeMap.ViewForAnnotation(native);
                view.Image = item.Image.ToNative();
                return;
            }
        }
    }
}

