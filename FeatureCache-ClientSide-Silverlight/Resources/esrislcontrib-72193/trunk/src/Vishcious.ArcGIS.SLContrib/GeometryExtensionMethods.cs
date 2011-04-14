using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using ESRI.ArcGIS.Client.Geometry;

namespace Vishcious.ArcGIS.SLContrib
{
    public static class GeometryExtensionMethods
    {
        public static void Offset(this Geometry geometry, double deltaX, double deltaY)
        {
            geometry.RequireArgument<Geometry>("geometry").NotNull<Geometry>();

            if (geometry is MapPoint)
                (geometry as MapPoint).Offset(deltaX, deltaY);
            else if (geometry is MultiPoint)
                (geometry as MultiPoint).Offset(deltaX, deltaY);
            else if (geometry is Polyline)
                (geometry as Polyline).Offset(deltaX, deltaY);
            else if (geometry is Polygon)
                (geometry as Polygon).Offset(deltaX, deltaY);
        }

        public static void Offset(this MapPoint point, double deltaX, double deltaY)
        {
            point.RequireArgument<MapPoint>("point").NotNull<MapPoint>();

            point.X = point.X + deltaX;
            point.Y = point.Y + deltaY;
        }

        public static void Offset(this MultiPoint multiPoint, double deltaX, double deltaY)
        {
            multiPoint.RequireArgument<MultiPoint>("multiPoint").NotNull<MultiPoint>();

            multiPoint.Points.ForEach<MapPoint>(p => p.Offset(deltaX, deltaY));
        }

        public static void Offset(this Polyline polyline, double deltaX, double deltaY)
        {
            polyline.RequireArgument<Polyline>("polyline").NotNull<Polyline>();

            polyline.Paths.ForEach<ESRI.ArcGIS.Client.Geometry.PointCollection>
                (
                    pts => pts.ForEach<MapPoint>(pt => pt.Offset(deltaX, deltaY))
                );
        }

        public static void Offset(this Polygon polygon, double deltaX, double deltaY)
        {
            polygon.RequireArgument<Polygon>("polygon").NotNull<Polygon>();

            polygon.Rings.ForEach<ESRI.ArcGIS.Client.Geometry.PointCollection>
                (
                    pts => pts.ForEach<MapPoint>(pt => pt.Offset(deltaX, deltaY))
                );
        }
    }
}
