using AgOpenGPS.Forms.Field;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace AgOpenGPS.Classes
{
    public class MQTTMessages
    {

        private readonly FormGPS mf;
        public MQTTMessages(FormGPS _f)
        {
            mf = _f;
        }

        public object GetLocalGPSStaticValues(FormGPS mf)
        {
            Object obj = new
            {
                mPerDegreeLat = mf.pn.mPerDegreeLat,
                mPerDegreeLon = mf.pn.mPerDegreeLon,
                lonStart = mf.pn.lonStart,
                latStart = mf.pn.latStart,
            };
            return obj;
        }

        private double PrepareValue(double value)
        {
            return Math.Round(value, 3);
        }
        private object CastVectPositionToObj(vec3 v)
        {
            return new { easting = PrepareValue(v.easting), northing = PrepareValue(v.northing), heading = PrepareValue(v.heading) };
        }

        private object CastVect2PositionToObj(vec2 v)
        {
            return new { easting = PrepareValue(v.easting), northing = PrepareValue(v.northing) };
        }
        private object GetLocalBoundry(FormGPS mf)
        {
            if (mf.bnd.bndList.Count > 0)
            {
                Object obj = new
                {
                    boundary = mf.bnd.bndList.Select(b => new
                    {
                        fence = b.fenceLine.Select(CastVectPositionToObj),
                        //turn = b.turnLine.Select(CastVectPositionToObj),
                    }),
                };
                return obj;
            }
            return null;
        }

        public object CreateCurrentABLine(FormGPS mf)
        {
            if (mf.trk.idx >= 0)
            {

                Object obj = new
                {
                    pointA = CastVectPositionToObj(mf.ABLine.currentLinePtA),
                    pointB = CastVectPositionToObj(mf.ABLine.currentLinePtB),
                };
                string message = JsonSerializer.Serialize(new MqttMessage() { msgType = MQTTMessageType.currentABLine, value = obj, timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") });
                var z = message.Length;
                return obj;
            }
            return null;
        }

        public object GetReferenceNavigationLines(FormGPS mf)
        {
            Object obj = new
            {
                abLines = mf.trk.gArr.Where(g => g.curvePts.Count == 0).Select(line => new
                {
                    name = line.name,
                    orgPointAB = new { pointA = CastVect2PositionToObj(line.ptA), pointB = CastVect2PositionToObj(line.ptB) },
                    extentPointAB = new { pointA = CastVect2PositionToObj(line.endPtA), pointB = CastVect2PositionToObj(line.endPtB) },
                }),
                curveLines = mf.trk.gArr.Where(g => g.curvePts.Count > 0).Select(line => new
                {
                    name = line.name,
                    orgPointAB = new { pointA = CastVect2PositionToObj(line.ptA), pointB = CastVect2PositionToObj(line.ptB) },
                    extentPointAB = new { pointA = CastVect2PositionToObj(line.endPtA), pointB = CastVect2PositionToObj(line.endPtB) },
                    curvePoints = line.curvePts.Select(CastVectPositionToObj),
                })
            };

            return obj;
        }

        public object GetTaskMetadata(FormGPS mf)
        {
            Object obj = new
            {
                taskGuid = mf.taskGuid,
                taskName = mf.displayFieldName,
                fieldGuid = mf.fieldGuid,
                vehicleGuid = mf.vehicleGuid,
                toolGuid = mf.toolGuid,
            };

            return obj;
        }

        public object GetTaskData(FormGPS mf)
        {
            var boundaryObjD = GetLocalBoundry(mf);
            var type = boundaryObjD?.GetType();
            if (type == null) return null;
            var prop = type.GetProperty("boundary");
            var boundary = prop.GetValue(boundaryObjD);

            Object obj = new
            {
                localCordsToGPSStatics = GetLocalGPSStaticValues(mf),
                boundary = boundary,
                taskMetadata = GetTaskMetadata(mf),
                //referenceNavigationLines = GetReferenceNavigationLines(mf),
            };

            return obj;

        }

        public object GetSecttions(FormGPS mf)
        {
            var sections = mf.section.Select((s, idx) => (s, idx)).Where(mft => mft.s.sectionWidth > 0 && mft.s.speedPixels > 0).ToList();
            var tool = mf.tool;

            Object obj = new
            {
                antennaPosition = new { easting = PrepareValue(mf.pn.fix.easting), northing = PrepareValue(mf.pn.fix.northing), heading = PrepareValue(mf.fixHeading) },
                sections = sections.Select(mft =>
                    new
                    {
                        mft.s.isSectionOn,
                        mft.s.isMappingOn,
                        mft.s.sectionBtnState,
                        speed = mf.SpeedKPH,
                        sectionNumber = mft.idx + 1,
                        position = new
                        {
                            //lastPosition = new { left = CastVectPositionToObj(mft.s.lastLeftPoint), right = CastVectPositionToObj(mft.s.lastRightPoint) },
                            current = new { left = CastVect2PositionToObj(mft.s.leftPoint), right = CastVect2PositionToObj(mft.s.rightPoint) }
                        }
                    }).ToList(),
                speed = mf.SpeedKPH,
            };
            return obj;
        }

        public bool SendTaskMetadata()
        {
            var obj = GetTaskData(mf);
            var c = mf.trk;
            string message = JsonSerializer.Serialize(new MqttMessage() { msgType = MQTTMessageType.taskMetadata, value = obj, timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), clientId = mf.clientId });
            var z = message.Length;
            var res = mf.customDataSender.SendDataViaMQTT(message);
            return true;
        }

        public void SendSectionsMessage()
        {
            var obj = GetSecttions(mf);
            string message = JsonSerializer.Serialize(new MqttMessage() { msgType = MQTTMessageType.sectionsInfo, value = obj, timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), clientId = mf.clientId });
            var z = message.Length;
            mf.customDataSender.SendDataViaMQTT(message);
        }

        private object GetMainTablesVFT(FormGPS mf)
        {
            var listOfRefFields = mf.formNewFieldCustom.GetListOfRefFields();
            var listOfTools = mf.formNewFieldCustom.GetListOfTools();
            var listOfVehicles = mf.formNewFieldCustom.GetListOfVehicles();

            Object obj = new
            {
                refFields = listOfRefFields.Select(rf => new { name = rf.name, guid = rf.guid, desc = rf.description, area = rf.area, modDate = rf.modDate, createDate = rf.createDate, lat = rf.lat, lon = rf.lon }),
                tools = listOfTools.Select(t => new { name = t.name, guid = t.guid, desc = t.description, modDate = t.modDate, createDate = t.createDate }),
                vehicles = listOfVehicles.Select(v => new { name = v.name, guid = v.guid, desc = v.description, modDate = v.modDate, createDate = v.createDate }),
            };
            return obj;
        }

        public void SendMainTablesVFT()
        {
            var obj = GetMainTablesVFT(mf);
            string message = JsonSerializer.Serialize(new MqttMessage() { msgType = MQTTMessageType.mainTablesVFT, value = obj, timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), clientId = mf.clientId });
            var z = message.Length;
            mf.customDataSender.SendDataViaMQTT(message);
        }
    }
}
