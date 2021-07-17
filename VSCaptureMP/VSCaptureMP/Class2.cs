/*
 * This file is part of VitalSignsCaptureMP v1.009.
 * Copyright (C) 2017-21 John George K., xeonfusion@users.sourceforge.net
 * Portions of code (C) 2015 Richard L. Grier
 
    VitalSignsCaptureMP is free software: you can redistribute it and/or modify
    it under the terms of the GNU Lesser General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    VitalSignsCaptureMP is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU Lesser General Public License for more details.

    You should have received a copy of the GNU Lesser General Public License
    along with VitalSignsCaptureMP.  If not, see <http://www.gnu.org/licenses/>.*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace VSCaptureMP
{
    
    public class IntelliVue
    {
        #region "Measurement State enumeration"
        private const UInt16 METRIC_OFF = 0x8000;
        public enum MeasurementState : UInt16
        {
            //non-CLS, but required
            INVALID = 0x8000,
            QUESTIONABLE = 0x4000,
            UNAVAILABLE = 0x2000,
            CALIBRATION_ONGOING = 0x1000,
            TEST_DATA = 0x800,
            DEMO_DATA = 0x400,
            VALIDATED_DATA = 0x80,
            EARLY_INDICATION = 0x40,
            MSMT_ONGOING = 0x20,
            MSMT_STATE_IN_ALARM = 0x2,
            MSMT_STATE_AL_INHIBITED = 0x1
        }
        #endregion
        #region "Object Classes enumeration"
        public enum ObjectClasses : UInt16
        {
            //non-CLS, but required  -also used for Alert Source
            NOM_MOC_VMO = 1,
            //VMO
            NOM_MOC_VMO_METRIC_NU = 6,
            //Numeric
            NOM_MOC_VMO_METRIC_SA_RT = 9,
            //Realtime Sample Array
            NOM_MOC_VMS_MDS = 33,
            //MDS
            NOM_MOC_VMS_MDS_COMPOS_SINGLE_BED = 35,
            //Composit Single Bed MDS
            NOM_MOC_VMS_MDS_SIMP = 37,
            //Simple MDS
            NOM_MOC_BATT = 41,
            //Battery
            NOM_MOC_PT_DEMOG = 42,
            //Patient Demographics
            NOM_MOC_VMO_AL_MON = 54,
            //Alert Monitor
            NOM_ACT_POLL_MDIB_DATA = 3094,
            //Poll Action
            NOM_NOTI_MDS_CREAT = 3334,
            //MDS Create
            NOM_NOTI_CONN_INDIC = 3351,
            //Connect Indication
            NOM_DEV_METER_CONC_SKIN_GAS = 4264,
            //Skin Gas
            NOM_DEV_METER_FLOW_BLD = 4284,
            //Blood Flow
            NOM_DEV_ANALY_CONC_GAS_MULTI_PARAM_MDS = 4113,
            //Gas Analyzer
            NOM_DEV_ANALY_CONC_GAS_MULTI_PARAM_VMD = 4114,
            //Gas
            NOM_DEV_METER_CONC_SKIN_GAS_MDS = 4265,
            //Skin Gas
            NOM_DEV_MON_PHYSIO_MULTI_PARAM_MDS = 4429,
            //Multi-Param
            NOM_DEV_PUMP_INFUS_MDS = 4449,
            //Pump Infus
            NOM_DEV_SYS_PT_VENT_MDS = 4465,
            //Ventilator
            NOM_DEV_SYS_PT_VENT_VMD = 4466,
            //Ventilator
            NOM_DEV_SYS_MULTI_MODAL_MDS = 4493,
            //Multi-Modal MDS
            NOM_DEV_SYS_MULTI_MODAL_VMD = 4494,
            //Multi-Modal
            NOM_DEV_SYS_VS_CONFIG_MDS = 5209,
            //config MDS
            NOM_DEV_SYS_VS_UNCONFIG_MDS = 5213,
            //unconfig MDS
            NOM_DEV_ANALY_SAT_O2_VMD = 4106,
            //sat O2
            NOM_DEV_ANALY_FLOW_AWAY_VMD = 4130,
            //Flow Away
            NOM_DEV_ANALY_CARD_OUTPUT_VMD = 4134,
            //CO
            NOM_DEV_ANALY_PRESS_BLD_VMD = 4174,
            //Press
            NOM_DEV_ANALY_RESP_RATE_VMD = 4186,
            //RR
            NOM_DEV_CALC_VMD = 4206,
            //Calculation
            NOM_DEV_ECG_VMD = 4262,
            //ECG
            NOM_DEV_METER_CONC_SKIN_GAS_VMD = 4266,
            //Skin Gas
            NOM_DEV_EEG_VMD = 4274,
            //EEG
            NOM_DEV_METER_TEMP_BLD_VMD = 4350,
            //Blood Temp
            NOM_DEV_METER_TEMP_VMD = 4366,
            //Temp
            NOM_DEV_MON_BLD_CHEM_MULTI_PARAM_VMD = 4398,
            //Bld Chem
            NOM_DEV_SYS_ANESTH_VMD = 4506,
            //Aneshesia
            NOM_DEV_GENERAL_VMD = 5122,
            //General
            NOM_DEV_ECG_RESP_VMD = 5130,
            //ECG-Resp
            NOM_DEV_ARRHY_VMD = 5134,
            //Arrythmia
            NOM_DEV_PULS_VMD = 5138,
            //Pulse
            NOM_DEV_ST_VMD = 5142,
            //ST
            NOM_DEV_CO2_VMD = 5146,
            //CO2
            NOM_DEV_PRESS_BLD_NONINV_VMD = 5150,
            //Noninv Press
            NOM_DEV_CEREB_PERF_VMD = 5154,
            //Cereb Perf
            NOM_DEV_CO2_CTS_VMD = 5158,
            //CO2 CTS
            NOM_DEV_CO2_TCUT_VMD = 5162,
            //TcCO2
            NOM_DEV_O2_VMD = 5166,
            //O2
            NOM_DEV_O2_CTS_VMD = 5170,
            //CTS
            NOM_DEV_O2_TCUT_VMD = 5174,
            //Tc02
            NOM_DEV_TEMP_DIFF_VMD = 5178,
            //Diff Temp
            NOM_DEV_CNTRL_VMD = 5182,
            //Control
            NOM_DEV_WEDGE_VMD = 5190,
            //Wedge
            NOM_DEV_O2_VEN_SAT_VMD = 5194,
            //O2 Vent Sat
            NOM_DEV_CARD_RATE_VMD = 5202,
            //HR
            NOM_DEV_PLETH_VMD = 5238,
            //Pleth
            NOM_SAT_O2_TONE_FREQ = 61448,
            //Private Attribute
            NOM_OBJ_HIF_KEY = 61584,
            //Key
            NOM_OBJ_DISP = 61616,
            //Display
            NOM_OBJ_SOUND_GEN = 61648,
            //Sound Generator
            NOM_OBJ_SETTING = 61649,
            //Setting
            NOM_OBJ_PRINTER = 61650,
            //Printer
            NOM_OBJ_EVENT = 61683,
            //Event
            NOM_OBJ_BATT_CHARGER = 61690,
            //Battery Charger
            NOM_OBJ_ECG_OUT = 61691,
            //ECG out
            NOM_OBJ_INPUT_DEV = 61692,
            //Input Device
            NOM_OBJ_NETWORK = 61693,
            //Network
            NOM_OBJ_QUICKLINK = 61694,
            //Quicklink Bar
            NOM_OBJ_SPEAKER = 61695,
            //Speaker
            NOM_OBJ_PUMP = 61716,
            //Pump
            NOM_OBJ_IR = 61717,
            //IR
            NOM_ACT_POLL_MDIB_DATA_EXT = 61755,
            //Extended Poll Action
            NOM_DEV_ANALY_PULS_CONT = 61800,
            //Puls Cont
            NOM_DEV_ANALY_BISPECTRAL_INDEX_VMD = 61806,
            //BIS
            NOM_DEV_HIRES_TREND = 61820,
            //Hires Trend
            NOM_DEV_HIRES_TREND_MDS = 61821,
            //Hires Trend
            NOM_DEV_HIRES_TREND_VMD = 61822,
            //Hires Trend
            NOM_DEV_MON_PT_EVENT_VMD = 61826,
            //Events
            NOM_DEV_DERIVED_MSMT = 61828,
            //Derived Measurement
            NOM_DEV_DERIVED_MSMT_MDS = 61829,
            //Derived Measurement
            NOM_DEV_DERIVED_MSMT_VMD = 61830,
            //Derived Measurement
            NOM_OBJ_SENSOR = 61902,
            //Sensor
            NOM_OBJ_XDUCR = 61903,
            //Transducer
            NOM_OBJ_CHAN_1 = 61916,
            //Channel 1
            NOM_OBJ_CHAN_2 = 61917,
            //Channel 2
            NOM_OBJ_AWAY_AGENT_1 = 61918,
            //Agent 1
            NOM_OBJ_AWAY_AGENT_2 = 61919,
            //Agent 2
            NOM_OBJ_HIF_MOUSE = 61983,
            //MOUSE
            NOM_OBJ_HIF_TOUCH = 61984,
            //TOUCH
            NOM_OBJ_HIF_SPEEDPOINT = 61985,
            //Speedpoint
            NOM_OBJ_HIF_ALARMBOX = 61986,
            //Alarmbox
            NOM_OBJ_BUS_I2C = 61987,
            //I2C Bus
            NOM_OBJ_CPU_SEC = 61988,
            //2nd CPU
            NOM_OBJ_LED = 61990,
            //LED
            NOM_OBJ_RELAY = 61991,
            //Relay
            NOM_OBJ_BATT_1 = 61996,
            //Battery 1
            NOM_OBJ_BATT_2 = 61997,
            //Battery 2
            NOM_OBJ_DISP_SEC = 61998,
            //2nd Display
            NOM_OBJ_AGM = 61999,
            //AGM
            NOM_OBJ_TELEMON = 62014,
            //TeleMon
            NOM_OBJ_XMTR = 62015,
            //Transmitter
            NOM_OBJ_CABLE = 62016,
            //Cable
            NOM_OBJ_TELEMETRY_XMTR = 62053,
            //Telemetry Transmitter
            NOM_OBJ_MMS = 62070,
            //MMS
            NOM_OBJ_DISP_THIRD = 62073,
            //Third Display
            NOM_OBJ_BATT = 62078,
            //Battery
            NOM_OBJ_BATT_TELE = 62091,
            //Battery Tele
            NOM_DEV_PROT_WATCH_CHAN = 62095,
            //Protocol Watch generic
            NOM_OBJ_PROT_WATCH_1 = 62097,
            //Protocol Watch Protocol No. 1
            NOM_OBJ_PROT_WATCH_2 = 62098,
            //Protocol Watch Protocol No. 2
            NOM_OBJ_PROT_WATCH_3 = 62099,
            //Protocol Watch Protocol No. 3
            NOM_OBJ_ECG_SYNC = 62147,
            //ECG Sync
            NOM_DEV_METAB_VMD = 62162,
            //Metabolism
            NOM_OBJ_SENSOR_O2_CO2 = 62165,
            //SENSOR O2 CO2
            NOM_OBJ_SRR_IF_1 = 62208,
            //SRR Interface 1
            NOM_OBJ_DISP_REMOTE = 62228
            //REMOTE DISPLAY
        }
        #endregion
        #region "Alert Source and Alarm Codes enumerations"
        public enum AlertSource : UInt16
        {
            NOM_ECG_LEAD_IG_LEAD_I = 1,
            NOM_ECG_LEAD_II = 2,
            NOM_ECG_LEAD_LA = 21,
            NOM_ECG_LEAD_RA = 22,
            NOM_ECG_LEAD_LL = 23,
            NOM_ECG_LEAD_fI = 24,
            NOM_ECG_LEAD_fE = 25,
            NOM_ECG_LEAD_fA = 27,
            NOM_ECG_LEAD_C = 66,
            NOM_ECG_LEAD_C1FR = 82,
            NOM_ECG_LEAD_C2FR = 83,
            NOM_ECG_LEAD_C3FR = 84,
            NOM_ECG_LEAD_C4FR = 85,
            NOM_ECG_LEAD_C5FR = 87,
            NOM_ECG_LEAD_C6FR = 88,
            NOM_ECG_LEAD_C7FR = 89,
            NOM_ECG_LEAD_C8FR = 90,
            NOM_ECG_LEAD_ES = 100,
            NOM_ECG_LEAD_AS = 101,
            NOM_ECG_LEAD_AI = 102,
            NOM_ECG_LEAD_RL = 115,
            NOM_ECG_LEAD_EASI_S = 116,
            NOM_ECG_ELEC_POTL = 256,
            NOM_ECG_ELEC_POTL_I = 257,
            NOM_ECG_ELEC_POTL_II = 258,
            NOM_ECG_ELEC_POTL_V1 = 259,
            NOM_ECG_ELEC_POTL_V2 = 260,
            NOM_ECG_ELEC_POTL_V3 = 261,
            NOM_ECG_ELEC_POTL_V4 = 262,
            NOM_ECG_ELEC_POTL_V5 = 263,
            NOM_ECG_ELEC_POTL_V6 = 264,
            NOM_ECG_ELEC_POTL_III = 317,
            NOM_ECG_ELEC_POTL_AVR = 318,
            NOM_ECG_ELEC_POTL_AVL = 319,
            NOM_ECG_ELEC_POTL_AVF = 320,
            NOM_ECG_ELEC_POTL_V = 323,
            NOM_ECG_ELEC_POTL_MCL = 331,
            NOM_ECG_ELEC_POTL_MCL1 = 332,
            NOM_ECG_AMPL_ST = 768,
            NOM_ECG_AMPL_ST_I = 769,
            NOM_ECG_AMPL_ST_II = 770,
            NOM_ECG_AMPL_ST_V1 = 771,
            NOM_ECG_AMPL_ST_V2 = 772,
            NOM_ECG_AMPL_ST_V3 = 773,
            NOM_ECG_AMPL_ST_V4 = 774,
            NOM_ECG_AMPL_ST_V5 = 775,
            NOM_ECG_AMPL_ST_V6 = 776,
            NOM_ECG_AMPL_ST_III = 829,
            NOM_ECG_AMPL_ST_AVR = 830,
            NOM_ECG_AMPL_ST_AVL = 831,
            NOM_ECG_AMPL_ST_AVF = 832,
            NOM_ECG_AMPL_ST_V = 835,
            NOM_ECG_AMPL_ST_MCL = 843,
            NOM_ECG_AMPL_ST_ES = 868,
            NOM_ECG_AMPL_ST_AS = 869,
            NOM_ECG_AMPL_ST_AI = 870,
            NOM_ECG_TIME_PD_QT_GL = 16160,
            NOM_ECG_TIME_PD_QTc = 16164,
            NOM_ECG_CARD_BEAT_RATE = 16770,
            NOM_ECG_CARD_BEAT_RATE_BTB = 16778,
            NOM_ECG_V_P_C_CNT = 16993,
            NOM_ECG_V_P_C_RATE = 16994,
            NOM_ECG_V_P_C_FREQ = 17000,
            NOM_PULS_RATE = 18442,
            NOM_PLETH_PULS_RATE = 18466,
            NOM_RES_VASC_SYS_INDEX = 18688,
            NOM_WK_LV_STROKE_INDEX = 18692,
            NOM_WK_RV_STROKE_INDEX = 18696,
            NOM_OUTPUT_CARD_INDEX = 18700,
            NOM_PRESS_BLD = 18944,
            NOM_PRESS_BLD_SYS = 18945,
            NOM_PRESS_BLD_DIA = 18946,
            NOM_PRESS_BLD_MEAN = 18947,
            NOM_PRESS_BLD_NONINV = 18948,
            NOM_PRESS_BLD_NONINV_SYS = 18949,
            NOM_PRESS_BLD_NONINV_DIA = 18950,
            NOM_PRESS_BLD_NONINV_MEAN = 18951,
            NOM_PRESS_BLD_AORT = 18956,
            NOM_PRESS_BLD_AORT_SYS = 18957,
            NOM_PRESS_BLD_AORT_DIA = 18958,
            NOM_PRESS_BLD_AORT_MEAN = 18959,
            NOM_PRESS_BLD_ART = 18960,
            NOM_PRESS_BLD_ART_SYS = 18961,
            NOM_PRESS_BLD_ART_DIA = 18962,
            NOM_PRESS_BLD_ART_MEAN = 18963,
            NOM_PRESS_BLD_ART_ABP = 18964,
            NOM_PRESS_BLD_ART_ABP_SYS = 18965,
            NOM_PRESS_BLD_ART_ABP_DIA = 18966,
            NOM_PRESS_BLD_ART_ABP_MEAN = 18967,
            NOM_PRESS_BLD_ART_PULM = 18972,
            NOM_PRESS_BLD_ART_PULM_SYS = 18973,
            NOM_PRESS_BLD_ART_PULM_DIA = 18974,
            NOM_PRESS_BLD_ART_PULM_MEAN = 18975,
            NOM_PRESS_BLD_ART_PULM_WEDGE = 18980,
            NOM_PRESS_BLD_ART_UMB = 18984,
            NOM_PRESS_BLD_ART_UMB_SYS = 18985,
            NOM_PRESS_BLD_ART_UMB_DIA = 18986,
            NOM_PRESS_BLD_ART_UMB_MEAN = 18987,
            NOM_PRESS_BLD_ATR_LEFT = 18992,
            NOM_PRESS_BLD_ATR_LEFT_SYS = 18993,
            NOM_PRESS_BLD_ATR_LEFT_DIA = 18994,
            NOM_PRESS_BLD_ATR_LEFT_MEAN = 18995,
            NOM_PRESS_BLD_ATR_RIGHT = 18996,
            NOM_PRESS_BLD_ATR_RIGHT_SYS = 18997,
            NOM_PRESS_BLD_ATR_RIGHT_DIA = 18998,
            NOM_PRESS_BLD_ATR_RIGHT_MEAN = 18999,
            NOM_PRESS_BLD_VEN_CENT = 19012,
            NOM_PRESS_BLD_VEN_CENT_SYS = 19013,
            NOM_PRESS_BLD_VEN_CENT_DIA = 19014,
            NOM_PRESS_BLD_VEN_CENT_MEAN = 19015,
            NOM_PRESS_BLD_VEN_UMB = 19016,
            NOM_PRESS_BLD_VEN_UMB_SYS = 19017,
            NOM_PRESS_BLD_VEN_UMB_DIA = 19018,
            NOM_PRESS_BLD_VEN_UMB_MEAN = 19019,
            NOM_SAT_O2_CONSUMP = 19200,
            NOM_OUTPUT_CARD = 19204,
            NOM_RES_VASC_PULM = 19236,
            NOM_RES_VASC_SYS = 19240,
            NOM_SAT_O2 = 19244,
            NOM_SAT_O2_ART = 19252,
            NOM_SAT_O2_VEN = 19260,
            NOM_SAT_DIFF_O2_ART_ALV = 19264,
            NOM_TEMP = 19272,
            NOM_TEMP_ART = 19280,
            NOM_TEMP_AWAY = 19284,
            NOM_TEMP_CORE = 19296,
            NOM_TEMP_ESOPH = 19300,
            NOM_TEMP_INJ = 19304,
            NOM_TEMP_NASOPH = 19308,
            NOM_TEMP_SKIN = 19316,
            NOM_TEMP_TYMP = 19320,
            NOM_TEMP_VEN = 19324,
            NOM_VOL_BLD_STROKE = 19332,
            NOM_WK_CARD_LEFT = 19344,
            NOM_WK_CARD_RIGHT = 19348,
            NOM_WK_LV_STROKE = 19356,
            NOM_WK_RV_STROKE = 19364,
            NOM_PULS_OXIM_PERF_REL = 19376,
            NOM_PLETH = 19380,
            NOM_PULS_OXIM_SAT_O2 = 19384,
            NOM_PULS_OXIM_SAT_O2_DIFF = 19396,
            NOM_PULS_OXIM_SAT_O2_ART_LEFT = 19400,
            NOM_PULS_OXIM_SAT_O2_ART_RIGHT = 19404,
            NOM_OUTPUT_CARD_CTS = 19420,
            NOM_VOL_VENT_L_END_SYS = 19460,
            NOM_GRAD_PRESS_BLD_AORT_POS_MAX = 19493,
            NOM_RESP = 20480,
            NOM_RESP_RATE = 20490,
            NOM_AWAY_RESP_RATE = 20498,
            NOM_CAPAC_VITAL = 20608,
            NOM_COMPL_LUNG = 20616,
            NOM_COMPL_LUNG_DYN = 20620,
            NOM_COMPL_LUNG_STATIC = 20624,
            NOM_CONC_AWAY_CO2 = 20628,
            NOM_CONC_AWAY_CO2_ET = 20636,
            NOM_CONC_AWAY_CO2_INSP_MIN = 20646,
            NOM_AWAY_CO2 = 20652,
            NOM_AWAY_CO2_ET = 20656,
            NOM_AWAY_CO2_INSP_MIN = 20666,
            NOM_CO2_TCUT = 20684,
            NOM_O2_TCUT = 20688,
            NOM_FLOW_AWAY = 20692,
            NOM_FLOW_AWAY_EXP_MAX = 20697,
            NOM_FLOW_AWAY_INSP_MAX = 20701,
            NOM_FLOW_CO2_PROD_RESP = 20704,
            NOM_IMPED_TTHOR = 20708,
            NOM_PRESS_RESP_PLAT = 20712,
            NOM_PRESS_AWAY = 20720,
            NOM_PRESS_AWAY_MIN = 20722,
            NOM_PRESS_AWAY_CTS_POS = 20724,
            NOM_PRESS_AWAY_NEG_MAX = 20729,
            NOM_PRESS_AWAY_END_EXP_POS_INTRINSIC = 20736,
            NOM_PRESS_AWAY_INSP = 20744,
            NOM_PRESS_AWAY_INSP_MAX = 20745,
            NOM_PRESS_AWAY_INSP_MEAN = 20747,
            NOM_RATIO_IE = 20760,
            NOM_RATIO_AWAY_DEADSP_TIDAL = 20764,
            NOM_RES_AWAY = 20768,
            NOM_RES_AWAY_EXP = 20772,
            NOM_RES_AWAY_INSP = 20776,
            NOM_TIME_PD_APNEA = 20784,
            NOM_VOL_AWAY_TIDAL = 20796,
            NOM_VOL_MINUTE_AWAY = 20808,
            NOM_VOL_MINUTE_AWAY_EXP = 20812,
            NOM_VOL_MINUTE_AWAY_INSP = 20816,
            NOM_CONC_AWAY_O2 = 20836,
            NOM_VENT_CONC_AWAY_O2_DELTA = 20840,
            NOM_VENT_CONC_AWAY_O2_EXP = 20844,
            NOM_VENT_AWAY_CO2_EXP = 20860,
            NOM_VENT_PRESS_AWAY_END_EXP_POS = 20904,
            NOM_VENT_VOL_AWAY_DEADSP = 20912,
            NOM_VENT_VOL_LUNG_TRAPD = 20920,
            NOM_VENT_CONC_AWAY_O2_INSP = 29848,
            NOM_VENT_FLOW_RATIO_PERF_ALV_INDEX = 20880,
            NOM_VENT_FLOW_INSP = 20876,
            NOM_VENT_CONC_AWAY_CO2_INSP = 20832,
            NOM_VENT_PRESS_OCCL = 20892,
            NOM_VENT_VOL_AWAY_DEADSP_REL = 20916,
            NOM_VENT_VOL_MINUTE_AWAY_MAND = 20940,
            NOM_COEF_GAS_TRAN = 20948,
            NOM_CONC_AWAY_DESFL = 20952,
            NOM_CONC_AWAY_ENFL = 20956,
            NOM_CONC_AWAY_HALOTH = 20960,
            NOM_CONC_AWAY_SEVOFL = 20964,
            NOM_CONC_AWAY_ISOFL = 20968,
            NOM_CONC_AWAY_N2O = 20976,
            NOM_CONC_AWAY_DESFL_ET = 21012,
            NOM_CONC_AWAY_ENFL_ET = 21016,
            NOM_CONC_AWAY_HALOTH_ET = 21020,
            NOM_CONC_AWAY_SEVOFL_ET = 21024,
            NOM_CONC_AWAY_ISOFL_ET = 21028,
            NOM_CONC_AWAY_N2O_ET = 21036,
            NOM_CONC_AWAY_DESFL_INSP = 21096,
            NOM_CONC_AWAY_ENFL_INSP = 21100,
            NOM_CONC_AWAY_HALOTH_INSP = 21104,
            NOM_CONC_AWAY_SEVOFL_INSP = 21108,
            NOM_CONC_AWAY_ISOFL_INSP = 21112,
            NOM_CONC_AWAY_N2O_INSP = 21120,
            NOM_CONC_AWAY_O2_INSP = 21124,
            NOM_VENT_TIME_PD_PPV = 21344,
            NOM_VENT_PRESS_RESP_PLAT = 21352,
            NOM_VENT_VOL_LEAK = 21360,
            NOM_VENT_VOL_LUNG_ALV = 21364,
            NOM_CONC_AWAY_O2_ET = 21368,
            NOM_CONC_AWAY_N2 = 21372,
            NOM_CONC_AWAY_N2_ET = 21376,
            NOM_CONC_AWAY_N2_INSP = 21380,
            NOM_CONC_AWAY_AGENT = 21384,
            NOM_CONC_AWAY_AGENT_ET = 21388,
            NOM_CONC_AWAY_AGENT_INSP = 21392,
            NOM_PRESS_CEREB_PERF = 22532,
            NOM_PRESS_INTRA_CRAN = 22536,
            NOM_PRESS_INTRA_CRAN_SYS = 22537,
            NOM_PRESS_INTRA_CRAN_DIA = 22538,
            NOM_PRESS_INTRA_CRAN_MEAN = 22539,
            NOM_SCORE_GLAS_COMA = 22656,
            NOM_SCORE_EYE_SUBSC_GLAS_COMA = 22658,
            NOM_SCORE_MOTOR_SUBSC_GLAS_COMA = 22659,
            NOM_SCORE_SUBSC_VERBAL_GLAS_COMA = 22660,
            NOM_CIRCUM_HEAD = 22784,
            NOM_TIME_PD_PUPIL_REACT_LEFT = 22820,
            NOM_TIME_PD_PUPIL_REACT_RIGHT = 22824,
            NOM_EEG_ELEC_POTL_CRTX = 22828,
            NOM_EMG_ELEC_POTL_MUSCL = 22844,
            NOM_EEG_FREQ_PWR_SPEC_CRTX_DOM_MEAN = 22908,
            NOM_EEG_FREQ_PWR_SPEC_CRTX_PEAK = 22916,
            NOM_EEG_FREQ_PWR_SPEC_CRTX_SPECTRAL_EDGE = 22920,
            NOM_EEG_PWR_SPEC_TOT = 22968,
            NOM_EEG_PWR_SPEC_ALPHA_REL = 22996,
            NOM_EEG_PWR_SPEC_BETA_REL = 23000,
            NOM_EEG_PWR_SPEC_DELTA_REL = 23004,
            NOM_EEG_PWR_SPEC_THETA_REL = 23008,
            NOM_FLOW_URINE_INSTANT = 26636,
            NOM_VOL_URINE_BAL_PD = 26660,
            NOM_VOL_URINE_COL = 26672,
            NOM_VOL_INFUS_ACTUAL_TOTAL = 26876,
            NOM_CONC_PH_ART = 28676,
            NOM_CONC_PCO2_ART = 28680,
            NOM_CONC_PO2_ART = 28684,
            NOM_CONC_HB_ART = 28692,
            NOM_CONC_HB_O2_ART = 28696,
            NOM_CONC_PO2_VEN = 28732,
            NOM_CONC_PH_VEN = 28724,
            NOM_CONC_PCO2_VEN = 28728,
            NOM_CONC_HB_O2_VEN = 28744,
            NOM_CONC_PH_URINE = 28772,
            NOM_CONC_NA_URINE = 28780,
            NOM_CONC_NA_SERUM = 28888,
            NOM_CONC_PH_GEN = 28932,
            NOM_CONC_HCO3_GEN = 28936,
            NOM_CONC_NA_GEN = 28940,
            NOM_CONC_K_GEN = 28944,
            NOM_CONC_GLU_GEN = 28948,
            NOM_CONC_CA_GEN = 28952,
            NOM_CONC_PCO2_GEN = 28992,
            NOM_CONC_CHLORIDE_GEN = 29032,
            NOM_BASE_EXCESS_BLD_ART = 29036,
            NOM_CONC_PO2_GEN = 29044,
            NOM_CONC_HCT_GEN = 29060,
            NOM_VENT_MODE_MAND_INTERMIT = 53290,
            NOM_TEMP_RECT = 57348,
            NOM_TEMP_BLD = 57364,
            NOM_TEMP_DIFF = 57368,
            NOM_METRIC_NOS = 61439,
            NOM_ECG_AMPL_ST_INDEX = 61501,
            NOM_TIME_TCUT_SENSOR = 61502,
            NOM_TEMP_TCUT_SENSOR = 61503,
            NOM_VOL_BLD_INTRA_THOR = 61504,
            NOM_VOL_BLD_INTRA_THOR_INDEX = 61505,
            NOM_VOL_LUNG_WATER_EXTRA_VASC = 61506,
            NOM_VOL_LUNG_WATER_EXTRA_VASC_INDEX = 61507,
            NOM_VOL_GLOBAL_END_DIA = 61508,
            NOM_VOL_GLOBAL_END_DIA_INDEX = 61509,
            NOM_CARD_FUNC_INDEX = 61510,
            NOM_OUTPUT_CARD_INDEX_CTS = 61511,
            NOM_VOL_BLD_STROKE_INDEX = 61512,
            NOM_VOL_BLD_STROKE_VAR = 61513,
            NOM_EEG_RATIO_SUPPRN = 61514,
            NOM_ELECTRODE_IMPED = 61515,
            NOM_EEG_BIS_SIG_QUAL_INDEX = 61517,
            NOM_EEG_BISPECTRAL_INDEX = 61518,
            NOM_GAS_TCUT = 61521,
            NOM_CONC_AWAY_SUM_MAC = 61533,
            NOM_CONC_AWAY_SUM_MAC_ET = 61534,
            NOM_CONC_AWAY_SUM_MAC_INSP = 61535,
            NOM_RES_VASC_PULM_INDEX = 61543,
            NOM_WK_CARD_LEFT_INDEX = 61544,
            NOM_WK_CARD_RIGHT_INDEX = 61545,
            NOM_SAT_O2_CONSUMP_INDEX = 61546,
            NOM_PRESS_AIR_AMBIENT = 61547,
            NOM_SAT_DIFF_O2_ART_VEN = 61548,
            NOM_SAT_O2_DELIVER = 61549,
            NOM_SAT_O2_DELIVER_INDEX = 61550,
            NOM_RATIO_SAT_O2_CONSUMP_DELIVER = 61551,
            NOM_RATIO_ART_VEN_SHUNT = 61552,
            NOM_AREA_BODY_SURFACE = 61553,
            NOM_INTENS_LIGHT = 61554,
            NOM_HEATING_PWR_TCUT_SENSOR = 61558,
            NOM_RATE_DIFF_CARD_BEAT_PULSE = 61560,
            NOM_VOL_INJ = 61561,
            NOM_VOL_THERMO_EXTRA_VASC_INDEX = 61562,
            NOM_NUM_CATHETER_CONST = 61564,
            NOM_PULS_OXIM_PERF_REL_LEFT = 61578,
            NOM_PULS_OXIM_PERF_REL_RIGHT = 61579,
            NOM_PULS_OXIM_PLETH_RIGHT = 61580,
            NOM_PULS_OXIM_PLETH_LEFT = 61581,
            NOM_CONC_BLD_UREA_NITROGEN = 61583,
            NOM_CONC_BASE_EXCESS_ECF = 61584,
            NOM_VENT_VOL_MINUTE_AWAY_SPONT = 61585,
            NOM_CONC_DIFF_HB_O2_ATR_VEN = 61586,
            NOM_PAT_WEIGHT = 61587,
            NOM_PAT_HEIGHT = 61588,
            NOM_CONC_AWAY_MAC = 61593,
            NOM_PULS_OXIM_PLETH_TELE = 61595,
            NOM_PULS_OXIM_SAT_O2_TELE = 61596,
            NOM_PULS_OXIM_PULS_RATE_TELE = 61597,
            NOM_PRESS_BLD_NONINV_TELE = 61600,
            NOM_PRESS_BLD_NONINV_TELE_SYS = 61601,
            NOM_PRESS_BLD_NONINV_TELE_DIA = 61602,
            NOM_PRESS_BLD_NONINV_TELE_MEAN = 61603,
            NOM_PRESS_GEN_1 = 61604,
            NOM_PRESS_GEN_1_SYS = 61605,
            NOM_PRESS_GEN_1_DIA = 61606,
            NOM_PRESS_GEN_1_MEAN = 61607,
            NOM_PRESS_GEN_2 = 61608,
            NOM_PRESS_GEN_2_SYS = 61609,
            NOM_PRESS_GEN_2_DIA = 61610,
            NOM_PRESS_GEN_2_MEAN = 61611,
            NOM_PRESS_GEN_3 = 61612,
            NOM_PRESS_GEN_3_SYS = 61613,
            NOM_PRESS_GEN_3_DIA = 61614,
            NOM_PRESS_GEN_3_MEAN = 61615,
            NOM_PRESS_GEN_4 = 61616,
            NOM_PRESS_GEN_4_SYS = 61617,
            NOM_PRESS_GEN_4_DIA = 61618,
            NOM_PRESS_GEN_4_MEAN = 61619,
            NOM_PRESS_INTRA_CRAN_1 = 61620,
            NOM_PRESS_INTRA_CRAN_1_SYS = 61621,
            NOM_PRESS_INTRA_CRAN_1_DIA = 61622,
            NOM_PRESS_INTRA_CRAN_1_MEAN = 61623,
            NOM_PRESS_INTRA_CRAN_2 = 61624,
            NOM_PRESS_INTRA_CRAN_2_SYS = 61625,
            NOM_PRESS_INTRA_CRAN_2_DIA = 61626,
            NOM_PRESS_INTRA_CRAN_2_MEAN = 61627,
            NOM_PRESS_BLD_ART_FEMORAL = 61628,
            NOM_PRESS_BLD_ART_FEMORAL_SYS = 61629,
            NOM_PRESS_BLD_ART_FEMORAL_DIA = 61630,
            NOM_PRESS_BLD_ART_FEMORAL_MEAN = 61631,
            NOM_PRESS_BLD_ART_BRACHIAL = 61632,
            NOM_PRESS_BLD_ART_BRACHIAL_SYS = 61633,
            NOM_PRESS_BLD_ART_BRACHIAL_DIA = 61634,
            NOM_PRESS_BLD_ART_BRACHIAL_MEAN = 61635,
            NOM_TEMP_VESICAL = 61636,
            NOM_TEMP_CEREBRAL = 61637,
            NOM_TEMP_AMBIENT = 61638,
            NOM_TEMP_GEN_1 = 61639,
            NOM_TEMP_GEN_2 = 61640,
            NOM_TEMP_GEN_3 = 61641,
            NOM_TEMP_GEN_4 = 61642,
            NOM_USOUND_CARD_BEAT_RATE_FETAL = 61643,
            NOM_USOUND_CARD_BEAT_RATE_FETAL_BTB = 61644,
            NOM_USOUND_CARD_BEAT_FETAL_SIG_QUAL_INDEX = 61645,
            NOM_ECG_CARD_BEAT_FETAL = 61646,
            NOM_ECG_CARD_BEAT_RATE_FETAL = 61647,
            NOM_ECG_CARD_BEAT_RATE_FETAL_BTB = 61648,
            NOM_ECG_CARD_BEAT_FETAL_SIG_QUAL_INDEX = 61649,
            NOM_TRIG_BEAT_FETAL = 61650,
            NOM_ECG_ELEC_POTL_FETAL = 61651,
            NOM_TOCO = 61652,
            NOM_STAT_COINCIDENCE = 61653,
            NOM_PRESS_INTRA_UTERAL = 61656,
            NOM_VOL_AWAY = 61663,
            NOM_VOL_AWAY_INSP_TIDAL = 61664,
            NOM_VOL_AWAY_EXP_TIDAL = 61665,
            NOM_AWAY_RESP_RATE_SPIRO = 61666,
            NOM_PULS_PRESS_VAR = 61667,
            NOM_PRESS_BLD_NONINV_PULS_RATE = 61669,
            NOM_RATIO_FETAL_MVMT_TOTAL = 61680,
            NOM_VENT_RESP_RATE_MAND = 61681,
            NOM_VENT_VOL_TIDAL_MAND = 61682,
            NOM_VENT_VOL_TIDAL_SPONT = 61683,
            NOM_CARDIAC_TROPONIN_I = 61684,
            NOM_CARDIO_PULMONARY_BYPASS_MODE = 61685,
            NOM_BNP = 61686,
            NOM_TIME_PD_RESP_PLAT = 61695,
            NOM_SAT_O2_VEN_CENT = 61696,
            NOM_SNR = 61697,
            NOM_HUMID = 61699,
            NOM_FRACT_EJECT = 61701,
            NOM_PERM_VASC_PULM_INDEX = 61702,
            NOM_TEMP_ORAL = 61704,
            NOM_TEMP_AXIL = 61708,
            NOM_TEMP_ORAL_PRED = 61712,
            NOM_TEMP_RECT_PRED = 61716,
            NOM_TEMP_AXIL_PRED = 61720,
            NOM_TEMP_AIR_INCUB = 61738,
            NOM_PULS_OXIM_PERF_REL_TELE = 61740,
            NOM_TEMP_PRED = 61760,
            NOM_SHUNT_RIGHT_LEFT = 61770,
            NOM_ECG_TIME_PD_QT_HEART_RATE = 61780,
            NOM_ECG_TIME_PD_QT_BASELINE = 61781,
            NOM_ECG_TIME_PD_QTc_DELTA = 61782,
            NOM_ECG_TIME_PD_QT_BASELINE_HEART_RATE = 61783,
            NOM_CONC_PH_CAP = 61784,
            NOM_CONC_PCO2_CAP = 61785,
            NOM_CONC_PO2_CAP = 61786,
            NOM_SAT_O2_CAP = 61793,
            NOM_CONC_MG_ION = 61787,
            NOM_CONC_MG_SER = 61788,
            NOM_CONC_tCA_SER = 61789,
            NOM_CONC_P_SER = 61790,
            NOM_CONC_CHLOR_SER = 61791,
            NOM_CONC_FE_GEN = 61792,
            NOM_CONC_AN_GAP = 61794,
            NOM_CONC_AN_GAP_CALC = 61857,
            NOM_CONC_ALB_SER = 61795,
            NOM_SAT_O2_ART_CALC = 61796,
            NOM_SAT_O2_VEN_CALC = 61798,
            NOM_SAT_O2_CAP_CALC = 61856,
            NOM_CONC_HB_CO_GEN = 29056,
            NOM_CONC_HB_FETAL = 61797,
            NOM_CONC_HB_MET_GEN = 29052,
            NOM_PLTS_CNT = 61799,
            NOM_WB_CNT = 61800,
            NOM_RB_CNT = 61801,
            NOM_RET_CNT = 61802,
            NOM_PLASMA_OSM = 61803,
            NOM_CONC_CREA_CLR = 61804,
            NOM_NSLOSS = 61805,
            NOM_CONC_CHOLESTEROL = 61806,
            NOM_CONC_TGL = 61807,
            NOM_CONC_HDL = 61808,
            NOM_CONC_LDL = 61809,
            NOM_CONC_UREA_GEN = 61810,
            NOM_CONC_CREA = 61811,
            NOM_CONC_LACT = 61812,
            NOM_CONC_BILI_TOT = 61815,
            NOM_CONC_PROT_SER = 61816,
            NOM_CONC_PROT_TOT = 61817,
            NOM_CONC_BILI_DIRECT = 61818,
            NOM_CONC_LDH = 61819,
            NOM_ES_RATE = 61820,
            NOM_CONC_PCT = 61821,
            NOM_CONC_CREA_KIN_MM = 61823,
            NOM_CONC_CREA_KIN_SER = 61824,
            NOM_CONC_CREA_KIN_MB = 61825,
            NOM_CONC_CHE = 61826,
            NOM_CONC_CRP = 61827,
            NOM_CONC_AST = 61828,
            NOM_CONC_AP = 61829,
            NOM_CONC_ALPHA_AMYLASE = 61830,
            NOM_CONC_GPT = 61831,
            NOM_CONC_GOT = 61832,
            NOM_CONC_GGT = 61833,
            NOM_TIME_PD_ACT = 61834,
            NOM_TIME_PD_PT = 61835,
            NOM_PT_INTL_NORM_RATIO = 61836,
            NOM_TIME_PD_aPTT_WB = 61837,
            NOM_TIME_PD_aPTT_PE = 61838,
            NOM_TIME_PD_PT_WB = 61839,
            NOM_TIME_PD_PT_PE = 61840,
            NOM_TIME_PD_THROMBIN = 61841,
            NOM_TIME_PD_COAGULATION = 61842,
            NOM_TIME_PD_THROMBOPLAS = 61843,
            NOM_FRACT_EXCR_NA = 61844,
            NOM_CONC_UREA_URINE = 61845,
            NOM_CONC_CREA_URINE = 61846,
            NOM_CONC_K_URINE = 61847,
            NOM_CONC_K_URINE_EXCR = 61848,
            NOM_CONC_OSM_URINE = 61849,
            NOM_CONC_GLU_URINE = 61855,
            NOM_CONC_CHLOR_URINE = 61850,
            NOM_CONC_PRO_URINE = 61851,
            NOM_CONC_CA_URINE = 61852,
            NOM_FLUID_DENS_URINE = 61853,
            NOM_CONC_HB_URINE = 61854,
            NOM_ENERGY_BAL = 61861,
            NOM_PULS_OXIM_SAT_O2_PRE_DUCTAL = 61888,
            NOM_PULS_OXIM_PERF_REL_PRE_DUCTAL = 61996,
            NOM_PULS_OXIM_SAT_O2_POST_DUCTAL = 61908,
            NOM_PULS_OXIM_PERF_REL_POST_DUCTAL = 61916,
            NOM_PRESS_GEN_5 = 62452,
            NOM_PRESS_GEN_5_SYS = 62453,
            NOM_PRESS_GEN_5_DIA = 62454,
            NOM_PRESS_GEN_5_MEAN = 62455,
            NOM_PRESS_GEN_6 = 62456,
            NOM_PRESS_GEN_6_SYS = 62457,
            NOM_PRESS_GEN_6_DIA = 62458,
            NOM_PRESS_GEN_6_MEAN = 62459,
            NOM_PRESS_GEN_7 = 62460,
            NOM_PRESS_GEN_7_SYS = 62461,
            NOM_PRESS_GEN_7_DIA = 62462,
            NOM_PRESS_GEN_7_MEAN = 62463,
            NOM_PRESS_GEN_8 = 62464,
            NOM_PRESS_GEN_8_SYS = 62465,
            NOM_PRESS_GEN_8_DIA = 62466,
            NOM_PRESS_GEN_8_MEAN = 62467,
            NOM_ECG_AMPL_ST_BASELINE_I = 62481,
            NOM_ECG_AMPL_ST_BASELINE_II = 62482,
            NOM_ECG_AMPL_ST_BASELINE_V1 = 62483,
            NOM_ECG_AMPL_ST_BASELINE_V2 = 62484,
            NOM_ECG_AMPL_ST_BASELINE_V3 = 62485,
            NOM_ECG_AMPL_ST_BASELINE_V4 = 62486,
            NOM_ECG_AMPL_ST_BASELINE_V5 = 62487,
            NOM_ECG_AMPL_ST_BASELINE_V6 = 62488,
            NOM_ECG_AMPL_ST_BASELINE_III = 62541,
            NOM_ECG_AMPL_ST_BASELINE_AVR = 62542,
            NOM_ECG_AMPL_ST_BASELINE_AVL = 62543,
            NOM_ECG_AMPL_ST_BASELINE_AVF = 62544,
            NOM_AGE = 63504,
            NOM_AGE_GEST = 63505,
            NOM_AWAY_CORR_COEF = 63508,
            NOM_AWAY_RESP_RATE_SPONT = 63509,
            NOM_AWAY_TC = 63510,
            NOM_BIRTH_LENGTH = 63512,
            NOM_BREATH_RAPID_SHALLOW_INDEX = 63513,
            NOM_C20_PER_C_INDEX = 63514,
            NOM_CARD_CONTRACT_HEATHER_INDEX = 63516,
            NOM_CONC_ALP = 63517,
            NOM_CONC_CA_GEN_NORM = 63522,
            NOM_CONC_CA_SER = 63524,
            NOM_CONC_CO2_TOT = 63525,
            NOM_CONC_CO2_TOT_CALC = 63526,
            NOM_CONC_CREA_SER = 63527,
            NOM_RESP_RATE_SPONT = 63528,
            NOM_CONC_GLO_SER = 63529,
            NOM_CONC_GLU_SER = 63530,
            NOM_CONC_HB_CORP_MEAN = 63532,
            NOM_CONC_K_SER = 63535,
            NOM_CONC_NA_EXCR = 63536,
            NOM_CONC_PCO2_ART_ADJ = 63538,
            NOM_CONC_PCO2_CAP_ADJ = 63539,
            NOM_CONC_PH_CAP_ADJ = 63543,
            NOM_CONC_PH_GEN_ADJ = 63544,
            NOM_CONC_PO2_ART_ADJ = 63547,
            NOM_CONC_PO2_CAP_ADJ = 63548,
            NOM_CREA_OSM = 63551,
            NOM_EEG_BURST_SUPPRN_INDEX = 63552,
            NOM_EEG_ELEC_POTL_CRTX_GAIN_LEFT = 63553,
            NOM_EEG_ELEC_POTL_CRTX_GAIN_RIGHT = 63554,
            NOM_EEG_FREQ_PWR_SPEC_CRTX_MEDIAN_LEFT = 63563,
            NOM_EEG_FREQ_PWR_SPEC_CRTX_MEDIAN_RIGHT = 63564,
            NOM_EEG_PWR_SPEC_ALPHA_ABS_LEFT = 63573,
            NOM_EEG_PWR_SPEC_ALPHA_ABS_RIGHT = 63574,
            NOM_EEG_PWR_SPEC_BETA_ABS_LEFT = 63579,
            NOM_EEG_PWR_SPEC_BETA_ABS_RIGHT = 63580,
            NOM_EEG_PWR_SPEC_DELTA_ABS_LEFT = 63587,
            NOM_EEG_PWR_SPEC_DELTA_ABS_RIGHT = 63588,
            NOM_EEG_PWR_SPEC_THETA_ABS_LEFT = 63593,
            NOM_EEG_PWR_SPEC_THETA_ABS_RIGHT = 63594,
            NOM_ELEC_EVOK_POTL_CRTX_ACOUSTIC_AAI = 63603,
            NOM_EXTRACT_O2_INDEX = 63605,
            NOM_FLOW_AWAY_AIR = 63607,
            NOM_FLOW_AWAY_EXP_ET = 63610,
            NOM_FLOW_AWAY_MAX_SPONT = 63613,
            NOM_FLOW_AWAY_TOT = 63617,
            NOM_FLOW_CO2_PROD_RESP_TIDAL = 63618,
            NOM_FLOW_URINE_PREV_24HR = 63619,
            NOM_FREE_WATER_CLR = 63620,
            NOM_HB_CORP_MEAN = 63621,
            NOM_HEATING_PWR_INCUBATOR = 63622,
            NOM_OUTPUT_CARD_INDEX_ACCEL = 63625,
            NOM_PTC_CNT = 63627,
            NOM_PULS_OXIM_PLETH_GAIN = 63629,
            NOM_RATIO_AWAY_RATE_VOL_AWAY = 63630,
            NOM_RATIO_BUN_CREA = 63631,
            NOM_RATIO_CONC_BLD_UREA_NITROGEN_CREA_CALC = 63632,
            NOM_RATIO_CONC_URINE_CREA_CALC = 63633,
            NOM_RATIO_CONC_URINE_CREA_SER = 63634,
            NOM_RATIO_CONC_URINE_NA_K = 63635,
            NOM_RATIO_PaO2_FIO2 = 63636,
            NOM_RATIO_TIME_PD_PT = 63637,
            NOM_RATIO_TIME_PD_PTT = 63638,
            NOM_RATIO_TRAIN_OF_FOUR = 63639,
            NOM_RATIO_URINE_SER_OSM = 63640,
            NOM_RES_AWAY_DYN = 63641,
            NOM_RESP_BREATH_ASSIST_CNT = 63642,
            NOM_RIGHT_HEART_FRACT_EJECT = 63643,
            NOM_TIME_PD_EVOK_REMAIN = 63648,
            NOM_TIME_PD_EXP = 63649,
            NOM_TIME_PD_FROM_LAST_MSMT = 63650,
            NOM_TIME_PD_INSP = 63651,
            NOM_TIME_PD_KAOLIN_CEPHALINE = 63652,
            NOM_TIME_PD_PTT = 63653,
            NOM_TRAIN_OF_FOUR_1 = 63655,
            NOM_TRAIN_OF_FOUR_2 = 63656,
            NOM_TRAIN_OF_FOUR_3 = 63657,
            NOM_TRAIN_OF_FOUR_4 = 63658,
            NOM_TRAIN_OF_FOUR_CNT = 63659,
            NOM_TWITCH_AMPL = 63660,
            NOM_UREA_SER = 63661,
            NOM_VENT_ACTIVE = 63664,
            NOM_VENT_AMPL_HFV = 63665,
            NOM_VENT_CONC_AWAY_AGENT_DELTA = 63666,
            NOM_VENT_CONC_AWAY_DESFL_DELTA = 63667,
            NOM_VENT_CONC_AWAY_ENFL_DELTA = 63668,
            NOM_VENT_CONC_AWAY_HALOTH_DELTA = 63669,
            NOM_VENT_CONC_AWAY_ISOFL_DELTA = 63670,
            NOM_VENT_CONC_AWAY_N2O_DELTA = 63671,
            NOM_VENT_CONC_AWAY_O2_CIRCUIT = 63672,
            NOM_VENT_CONC_AWAY_SEVOFL_DELTA = 63673,
            NOM_VENT_PRESS_AWAY_END_EXP_POS_LIMIT_LO = 63674,
            NOM_VENT_PRESS_AWAY_PV = 63676,
            NOM_VENT_TIME_PD_RAMP = 63677,
            NOM_VENT_VOL_AWAY_INSP_TIDAL_HFV = 63678,
            NOM_VENT_VOL_TIDAL_HFV = 63679,
            NOM_VOL_AWAY_EXP_TIDAL_SPONT = 63682,
            NOM_VOL_AWAY_TIDAL_PSV = 63683,
            NOM_VOL_CORP_MEAN = 63684,
            NOM_VOL_FLUID_THORAC = 63685,
            NOM_VOL_FLUID_THORAC_INDEX = 63686,
            NOM_VOL_LVL_LIQUID_BOTTLE_AGENT = 63687,
            NOM_VOL_LVL_LIQUID_BOTTLE_DESFL = 63688,
            NOM_VOL_LVL_LIQUID_BOTTLE_ENFL = 63689,
            NOM_VOL_LVL_LIQUID_BOTTLE_HALOTH = 63690,
            NOM_VOL_LVL_LIQUID_BOTTLE_ISOFL = 63691,
            NOM_VOL_LVL_LIQUID_BOTTLE_SEVOFL = 63692,
            NOM_VOL_MINUTE_AWAY_INSP_HFV = 63693,
            NOM_VOL_URINE_BAL_PD_INSTANT = 63694,
            NOM_VOL_URINE_SHIFT = 63695,
            NOM_VOL_VENT_L_END_SYS_INDEX = 63697,
            NOM_WEIGHT_URINE_COL = 63699,
            NOM_SAT_O2_TISSUE = 63840,
            NOM_CEREB_STATE_INDEX = 63841,
            NOM_SAT_O2_GEN_1 = 63842,
            NOM_SAT_O2_GEN_2 = 63843,
            NOM_SAT_O2_GEN_3 = 63844,
            NOM_SAT_O2_GEN_4 = 63845,
            NOM_TEMP_CORE_GEN_1 = 63846,
            NOM_TEMP_CORE_GEN_2 = 63847,
            NOM_PRESS_BLD_DIFF = 63848,
            NOM_PRESS_BLD_DIFF_GEN_1 = 63852,
            NOM_PRESS_BLD_DIFF_GEN_2 = 63856,
            NOM_FLOW_PUMP_HEART_LUNG_MAIN = 63860,
            NOM_FLOW_PUMP_HEART_LUNG_SLAVE = 63861,
            NOM_FLOW_PUMP_HEART_LUNG_SUCTION = 63862,
            NOM_FLOW_PUMP_HEART_LUNG_AUX = 63863,
            NOM_FLOW_PUMP_HEART_LUNG_CARDIOPLEGIA_MAIN = 63864,
            NOM_FLOW_PUMP_HEART_LUNG_CARDIOPLEGIA_SLAVE = 63865,
            NOM_TIME_PD_PUMP_HEART_LUNG_AUX_SINCE_START = 63866,
            NOM_TIME_PD_PUMP_HEART_LUNG_AUX_SINCE_STOP = 63867,
            NOM_VOL_DELIV_PUMP_HEART_LUNG_AUX = 63868,
            NOM_VOL_DELIV_TOTAL_PUMP_HEART_LUNG_AUX = 63869,
            NOM_TIME_PD_PLEGIA_PUMP_HEART_LUNG_AUX = 63870,
            NOM_TIME_PD_PUMP_HEART_LUNG_CARDIOPLEGIA_MAIN_SINCE_START = 63871,
            NOM_TIME_PD_PUMP_HEART_LUNG_CARDIOPLEGIA_MAIN_SINCE_STOP = 63872,
            NOM_VOL_DELIV_PUMP_HEART_LUNG_CARDIOPLEGIA_MAIN = 63873,
            NOM_VOL_DELIV_TOTAL_PUMP_HEART_LUNG_CARDIOPLEGIA_MAIN = 63874,
            NOM_TIME_PD_PLEGIA_PUMP_HEART_LUNG_CARDIOPLEGIA_MAIN = 63875,
            NOM_TIME_PD_PUMP_HEART_LUNG_CARDIOPLEGIA_SLAVE_SINCE_START = 63876,
            NOM_TIME_PD_PUMP_HEART_LUNG_CARDIOPLEGIA_SLAVE_SINCE_STOP = 63877,
            NOM_VOL_DELIV_PUMP_HEART_LUNG_CARDIOPLEGIA_SLAVE = 63878,
            NOM_VOL_DELIV_TOTAL_PUMP_HEART_LUNG_CARDIOPLEGIA_SLAVE = 63879,
            NOM_TIME_PD_PLEGIA_PUMP_HEART_LUNG_CARDIOPLEGIA_SLAVE = 63880,
            NOM_RATIO_INSP_TOTAL_BREATH_SPONT = 63888,
            NOM_VENT_PRESS_AWAY_END_EXP_POS_TOTAL = 63889,
            NOM_COMPL_LUNG_PAV = 63890,
            NOM_RES_AWAY_PAV = 63891,
            NOM_RES_AWAY_EXP_TOTAL = 63892,
            NOM_ELAS_LUNG_PAV = 63893,
            NOM_BREATH_RAPID_SHALLOW_INDEX_NORM = 63894
        }
        public enum AlarmCodes : UInt16
        {
            NOM_EVT_ABSENT = 4,
            NOM_EVT_CONTAM = 14,
            NOM_EVT_DISCONN = 22,
            NOM_EVT_DISTURB = 24,
            NOM_EVT_EMPTY = 26,
            NOM_EVT_ERRATIC = 32,
            NOM_EVT_EXH = 36,
            NOM_EVT_FAIL = 38,
            NOM_EVT_HI = 40,
            NOM_EVT_IRREG = 58,
            NOM_EVT_LO = 62,
            NOM_EVT_MALF = 70,
            NOM_EVT_NOISY = 74,
            NOM_EVT_OBSTRUC = 80,
            NOM_EVT_REVERSED = 96,
            NOM_EVT_SUST = 106,
            NOM_EVT_UNAVAIL = 110,
            NOM_EVT_UNDEF = 112,
            NOM_EVT_WARMING = 124,
            NOM_EVT_WEAK = 128,
            NOM_EVT_BREATH_ABSENT = 136,
            NOM_EVT_CALIB_FAIL = 138,
            NOM_EVT_CONFIG_ERR = 142,
            NOM_EVT_RANGE_ERR = 164,
            NOM_EVT_RANGE_OVER = 166,
            NOM_EVT_SRC_ABSENT = 174,
            NOM_EVT_SYNCH_ERR = 182,
            NOM_EVT_BATT_LO = 194,
            NOM_EVT_BATT_PROB = 198,
            NOM_EVT_CUFF_NOT_DEFLATED = 230,
            NOM_EVT_CUFF_INFLAT_OVER = 232,
            NOM_EVT_EQUIP_MALF = 242,
            NOM_EVT_TUBE_OCCL = 250,
            NOM_EVT_GAS_AGENT_IDENT_MALF = 258,
            NOM_EVT_LEAD_DISCONN = 268,
            NOM_EVT_LEADS_OFF = 274,
            NOM_EVT_O2_SUPPLY_LO = 296,
            NOM_EVT_OPTIC_MODULE_ABSENT = 298,
            NOM_EVT_OPTIC_MODULE_DEFECT = 300,
            NOM_EVT_SENSOR_DISCONN = 308,
            NOM_EVT_SENSOR_MALF = 310,
            NOM_EVT_SENSOR_PROB = 312,
            NOM_EVT_SW_VER_UNK = 322,
            NOM_EVT_TUBE_DISCONN = 326,
            NOM_EVT_TUBE_OBSTRUC = 330,
            NOM_EVT_XDUCR_DISCONN = 336,
            NOM_EVT_XDUCR_MALF = 338,
            NOM_EVT_INTENS_LIGHT_ERR = 350,
            NOM_EVT_MSMT_DISCONN = 352,
            NOM_EVT_MSMT_ERR = 354,
            NOM_EVT_MSMT_FAIL = 356,
            NOM_EVT_MSMT_INOP = 358,
            NOM_EVT_MSMT_INTERRUP = 362,
            NOM_EVT_MSMT_RANGE_OVER = 364,
            NOM_EVT_MSMT_RANGE_UNDER = 366,
            NOM_EVT_SIG_LO = 380,
            NOM_EVT_SIG_UNANALYZEABLE = 384,
            NOM_EVT_TEMP_HI_GT_LIM = 394,
            NOM_EVT_UNSUPPORTED = 400,
            NOM_EVT_WAVE_ARTIF_ERR = 432,
            NOM_EVT_WAVE_SIG_QUAL_ERR = 434,
            NOM_EVT_MSMT_INTERF_ERR = 436,
            NOM_EVT_WAVE_OSCIL_ABSENT = 442,
            NOM_EVT_VOLTAGE_OUT_OF_RANGE = 460,
            NOM_EVT_INCOMPAT = 600,
            NOM_EVT_ADVIS_CHK = 6658,
            NOM_EVT_ADVIS_CALIB_AND_ZERO_CHK = 6664,
            NOM_EVT_ADVIS_CONFIG_CHK = 6666,
            NOM_EVT_ADVIS_SETTINGS_CHK = 6668,
            NOM_EVT_ADVIS_SETUP_CHK = 6670,
            NOM_EVT_ADVIS_SRC_CHK = 6672,
            NOM_EVT_BATT_COND = 6676,
            NOM_EVT_BATT_REPLACE = 6678,
            NOM_EVT_ADVIS_CABLE_CHK = 6680,
            NOM_EVT_ADVIS_GAS_AGENT_CHK = 6688,
            NOM_EVT_ADVIS_LEAD_CHK = 6690,
            NOM_EVT_ADVIS_SENSOR_CHK = 6696,
            NOM_EVT_ADVIS_GAIN_DECR = 6704,
            NOM_EVT_ADVIS_GAIN_INCR = 6706,
            NOM_EVT_ADVIS_UNIT_CHK = 6710,
            NOM_EVT_APNEA = 3072,
            NOM_EVT_ECG_ASYSTOLE = 3076,
            NOM_EVT_ECG_BEAT_MISSED = 3078,
            NOM_EVT_ECG_BIGEM = 3082,
            NOM_EVT_ECG_BRADY_EXTREME = 3086,
            NOM_EVT_ECG_PACING_NON_CAPT = 3102,
            NOM_EVT_ECG_PAUSE = 3108,
            NOM_EVT_ECG_TACHY_EXTREME = 3122,
            NOM_EVT_ECG_CARD_BEAT_RATE_IRREG = 3158,
            NOM_EVT_ECG_PACER_NOT_PACING = 3182,
            NOM_EVT_ECG_SV_TACHY = 3192,
            NOM_EVT_ECG_V_P_C_RonT = 3206,
            NOM_EVT_ECG_V_P_C_MULTIFORM = 3208,
            NOM_EVT_ECG_V_P_C_PAIR = 3210,
            NOM_EVT_ECG_V_P_C_RUN = 3212,
            NOM_EVT_ECG_V_RHY = 3220,
            NOM_EVT_ECG_V_TACHY = 3224,
            NOM_EVT_ECG_V_TACHY_NON_SUST = 3226,
            NOM_EVT_ECG_V_TRIGEM = 3236,
            NOM_EVT_DESAT = 3246,
            NOM_EVT_ECG_V_P_C_RATE = 3252,
            NOM_EVT_STAT_AL_OFF = 6144,
            NOM_EVT_STAT_BATT_CHARGING = 6150,
            NOM_EVT_STAT_CALIB_MODE = 6152,
            NOM_EVT_STAT_CALIB_RUNNING = 6154,
            NOM_EVT_STAT_CALIB_INVIVO_RUNNING = 6156,
            NOM_EVT_STAT_CALIB_LIGHT_RUNNING = 6158,
            NOM_EVT_STAT_CALIB_PREINS_RUNNING = 6160,
            NOM_EVT_STAT_SELFTEST_RUNNING = 6164,
            NOM_EVT_STAT_ZERO_RUNNING = 6170,
            NOM_EVT_STAT_OPT_MOD_SENSOR_CONN = 6172,
            NOM_EVT_STAT_OPT_MOD_SENSOR_WARMING = 6174,
            NOM_EVT_STAT_SENSOR_WARMING = 6176,
            NOM_EVT_STAT_WARMING = 6178,
            NOM_EVT_STAT_ECG_AL_ALL_OFF = 6182,
            NOM_EVT_STAT_ECG_AL_SOME_OFF = 6184,
            NOM_EVT_STAT_LEARN = 6224,
            NOM_EVT_STAT_OFF = 6226,
            NOM_EVT_STAT_STANDBY = 6228,
            NOM_EVT_STAT_DISCONN = 6256,
            NOM_EVT_ADVIS_CALIB_REQD = 6662,
            NOM_EVT_ECG_V_FIB_TACHY = 61444,
            NOM_EVT_WAIT_CAL = 61678,
            NOM_EVT_ADVIS_CHANGE_SITE = 61682,
            NOM_EVT_ADVIS_CHECK_SITE_TIME = 61684,
            NOM_EVT_STAT_FW_UPDATE_IN_PROGRESS = 61688,
            NOM_EVT_EXT_DEV_AL_CODE_1 = 61690,
            NOM_EVT_EXT_DEV_AL_CODE_2 = 61692,
            NOM_EVT_EXT_DEV_AL_CODE_3 = 61694,
            NOM_EVT_EXT_DEV_AL_CODE_4 = 61696,
            NOM_EVT_EXT_DEV_AL_CODE_5 = 61698,
            NOM_EVT_EXT_DEV_AL_CODE_6 = 61700,
            NOM_EVT_EXT_DEV_AL_CODE_7 = 61702,
            NOM_EVT_EXT_DEV_AL_CODE_8 = 61704,
            NOM_EVT_EXT_DEV_AL_CODE_9 = 61706,
            NOM_EVT_EXT_DEV_AL_CODE_10 = 61708,
            NOM_EVT_EXT_DEV_AL_CODE_11 = 61710,
            NOM_EVT_EXT_DEV_AL_CODE_12 = 61712,
            NOM_EVT_EXT_DEV_AL_CODE_13 = 61714,
            NOM_EVT_EXT_DEV_AL_CODE_14 = 61716,
            NOM_EVT_EXT_DEV_AL_CODE_15 = 61718,
            NOM_EVT_EXT_DEV_AL_CODE_16 = 61720,
            NOM_EVT_EXT_DEV_AL_CODE_17 = 61722,
            NOM_EVT_EXT_DEV_AL_CODE_18 = 61724,
            NOM_EVT_EXT_DEV_AL_CODE_19 = 61726,
            NOM_EVT_EXT_DEV_AL_CODE_20 = 61728,
            NOM_EVT_EXT_DEV_AL_CODE_21 = 61730,
            NOM_EVT_EXT_DEV_AL_CODE_22 = 61732,
            NOM_EVT_EXT_DEV_AL_CODE_23 = 61734,
            NOM_EVT_EXT_DEV_AL_CODE_24 = 61736,
            NOM_EVT_EXT_DEV_AL_CODE_25 = 61738,
            NOM_EVT_EXT_DEV_AL_CODE_26 = 61740,
            NOM_EVT_EXT_DEV_AL_CODE_27 = 61742,
            NOM_EVT_EXT_DEV_AL_CODE_28 = 61744,
            NOM_EVT_EXT_DEV_AL_CODE_29 = 61746,
            NOM_EVT_EXT_DEV_AL_CODE_30 = 61748,
            NOM_EVT_EXT_DEV_AL_CODE_31 = 61750,
            NOM_EVT_EXT_DEV_AL_CODE_32 = 61752,
            NOM_EVT_EXT_DEV_AL_CODE_33 = 61754,
            NOM_EVT_ST_MULTI = 61756,
            NOM_EVT_ADVIS_BSA_REQD = 61760,
            NOM_EVT_ADVIS_PRESUMED_CVP = 61762,
            NOM_EVT_MSMT_UNSUPPORTED = 61764,
            NOM_EVT_BRADY = 61766,
            NOM_EVT_TACHY = 61768,
            NOM_EVT_ADVIS_CHANGE_SCALE = 61770,
            NOM_EVT_MSMT_RESTART = 61772,
            NOM_EVT_TOO_MANY_AGENTS = 61774,
            NOM_EVT_STAT_PULSE_SRC_RANGE_OVER = 61778,
            NOM_EVT_STAT_PRESS_SRC_RANGE_OVER = 61780,
            NOM_EVT_MUSCLE_NOISE = 61782,
            NOM_EVT_LINE_NOISE = 61784,
            NOM_EVT_IMPED_HI = 61786,
            NOM_EVT_AGENT_MIX = 61788,
            NOM_EVT_IMPEDS_HI = 61790,
            NOM_EVT_ADVIS_PWR_HI = 61792,
            NOM_EVT_ADVIS_PWR_OFF = 61794,
            NOM_EVT_ADVIS_PWR_OVER = 61796,
            NOM_EVT_ADVIS_DEACT = 61798,
            NOM_EVT_CO_WARNING = 61800,
            NOM_EVT_ADVIS_NURSE_CALL = 61802,
            NOM_EVT_COMP_MALF = 61804,
            NOM_EVT_AGENT_MEAS_MALF = 61806,
            NOM_EVT_ADVIS_WATER_TRAP_CHK = 61808,
            NOM_EVT_STAT_AGENT_CALC_RUNNING = 61810,
            NOM_EVT_ADVIS_ADAPTER_CHK = 61814,
            NOM_EVT_ADVIS_PUMP_OFF = 61816,
            NOM_EVT_ZERO_FAIL = 61818,
            NOM_EVT_ADVIS_ZERO_REQD = 61820,
            NOM_EVT_EXTR_HI = 61830,
            NOM_EVT_EXTR_LO = 61832,
            NOM_EVT_LEAD_DISCONN_YELLOW = 61833,
            NOM_EVT_LEAD_DISCONN_RED = 61834,
            NOM_EVT_CUFF_INFLAT_OVER_YELLOW = 61835,
            NOM_EVT_CUFF_INFLAT_OVER_RED = 61836,
            NOM_EVT_CUFF_NOT_DEFLATED_YELLOW = 61837,
            NOM_EVT_CUFF_NOT_DEFLATED_RED = 61838,
            NOM_EVT_ADVIS_ACTION_REQD = 61840,
            NOM_EVT_OUT_OF_AREA = 61842,
            NOM_EVT_LEADS_DISCONN = 61844,
            NOM_EVT_DEV_ASSOC_CHK = 61846,
            NOM_EVT_SYNCH_UNSUPPORTED = 61848,
            NOM_EVT_ECG_ADVIS_SRC_CHK = 61850,
            NOM_EVT_ALARM_TECH = 61852,
            NOM_EVT_ALARM_TECH_YELLOW = 61854,
            NOM_EVT_ALARM_TECH_RED = 61856,
            NOM_EVT_ALARM_MED_YELLOW_SHORT = 61858,
            NOM_EVT_ALARM_MED_YELLOW = 61860,
            NOM_EVT_ALARM_MED_RED = 61862,
            NOM_EVT_TELE_EQUIP_MALF = 61874,
            NOM_EVT_SYNCH_ERR_ECG = 61876,
            NOM_EVT_SYNCH_ERR_SPO2T = 61878,
            NOM_EVT_ADVIS_ACTION_REQD_YELLOW = 61880,
            NOM_EVT_ADVIS_NBP_SEQ_COMPLETED = 61882,
            NOM_EVT_PACER_OUTPUT_LO = 61884,
            NOM_EVT_ALARM_MORE_TECH = 61886,
            NOM_EVT_ALARM_MORE_TECH_YELLOW = 61888,
            NOM_EVT_ALARM_MORE_TECH_RED = 61890,
            NOM_EVT_ADVIS_PATIENT_CONFLICT = 61892,
            NOM_EVT_SENSOR_REPLACE = 61894,
            NOM_EVT_ECG_ATR_FIB = 61896,
            NOM_EVT_LIMITED_CONNECTIVITY = 61900,
            NOM_EVT_DISABLED = 61924,
            NOM_EVT_ECG_ABSENT = 61926,
            NOM_EVT_SRR_INTERF = 61928,
            NOM_EVT_SRR_INVALID_CHAN = 61930,
            NOM_EVT_EXT_DEV_DEMO = 62032,
            NOM_EVT_EXT_DEV_MONITORING = 62034
        }
        public enum PublicAlarmCodes : Int32
        {
            NOM_EVT_ABSENT = 4,
            NOM_EVT_CONTAM = 14,
            NOM_EVT_DISCONN = 22,
            NOM_EVT_DISTURB = 24,
            NOM_EVT_EMPTY = 26,
            NOM_EVT_ERRATIC = 32,
            NOM_EVT_EXH = 36,
            NOM_EVT_FAIL = 38,
            NOM_EVT_HI = 40,
            NOM_EVT_IRREG = 58,
            NOM_EVT_LO = 62,
            NOM_EVT_MALF = 70,
            NOM_EVT_NOISY = 74,
            NOM_EVT_OBSTRUC = 80,
            NOM_EVT_REVERSED = 96,
            NOM_EVT_SUST = 106,
            NOM_EVT_UNAVAIL = 110,
            NOM_EVT_UNDEF = 112,
            NOM_EVT_WARMING = 124,
            NOM_EVT_WEAK = 128,
            NOM_EVT_BREATH_ABSENT = 136,
            NOM_EVT_CALIB_FAIL = 138,
            NOM_EVT_CONFIG_ERR = 142,
            NOM_EVT_RANGE_ERR = 164,
            NOM_EVT_RANGE_OVER = 166,
            NOM_EVT_SRC_ABSENT = 174,
            NOM_EVT_SYNCH_ERR = 182,
            NOM_EVT_BATT_LO = 194,
            NOM_EVT_BATT_PROB = 198,
            NOM_EVT_CUFF_NOT_DEFLATED = 230,
            NOM_EVT_CUFF_INFLAT_OVER = 232,
            NOM_EVT_EQUIP_MALF = 242,
            NOM_EVT_TUBE_OCCL = 250,
            NOM_EVT_GAS_AGENT_IDENT_MALF = 258,
            NOM_EVT_LEAD_DISCONN = 268,
            NOM_EVT_LEADS_OFF = 274,
            NOM_EVT_O2_SUPPLY_LO = 296,
            NOM_EVT_OPTIC_MODULE_ABSENT = 298,
            NOM_EVT_OPTIC_MODULE_DEFECT = 300,
            NOM_EVT_SENSOR_DISCONN = 308,
            NOM_EVT_SENSOR_MALF = 310,
            NOM_EVT_SENSOR_PROB = 312,
            NOM_EVT_SW_VER_UNK = 322,
            NOM_EVT_TUBE_DISCONN = 326,
            NOM_EVT_TUBE_OBSTRUC = 330,
            NOM_EVT_XDUCR_DISCONN = 336,
            NOM_EVT_XDUCR_MALF = 338,
            NOM_EVT_INTENS_LIGHT_ERR = 350,
            NOM_EVT_MSMT_DISCONN = 352,
            NOM_EVT_MSMT_ERR = 354,
            NOM_EVT_MSMT_FAIL = 356,
            NOM_EVT_MSMT_INOP = 358,
            NOM_EVT_MSMT_INTERRUP = 362,
            NOM_EVT_MSMT_RANGE_OVER = 364,
            NOM_EVT_MSMT_RANGE_UNDER = 366,
            NOM_EVT_SIG_LO = 380,
            NOM_EVT_SIG_UNANALYZEABLE = 384,
            NOM_EVT_TEMP_HI_GT_LIM = 394,
            NOM_EVT_UNSUPPORTED = 400,
            NOM_EVT_WAVE_ARTIF_ERR = 432,
            NOM_EVT_WAVE_SIG_QUAL_ERR = 434,
            NOM_EVT_MSMT_INTERF_ERR = 436,
            NOM_EVT_WAVE_OSCIL_ABSENT = 442,
            NOM_EVT_VOLTAGE_OUT_OF_RANGE = 460,
            NOM_EVT_INCOMPAT = 600,
            NOM_EVT_ADVIS_CHK = 6658,
            NOM_EVT_ADVIS_CALIB_AND_ZERO_CHK = 6664,
            NOM_EVT_ADVIS_CONFIG_CHK = 6666,
            NOM_EVT_ADVIS_SETTINGS_CHK = 6668,
            NOM_EVT_ADVIS_SETUP_CHK = 6670,
            NOM_EVT_ADVIS_SRC_CHK = 6672,
            NOM_EVT_BATT_COND = 6676,
            NOM_EVT_BATT_REPLACE = 6678,
            NOM_EVT_ADVIS_CABLE_CHK = 6680,
            NOM_EVT_ADVIS_GAS_AGENT_CHK = 6688,
            NOM_EVT_ADVIS_LEAD_CHK = 6690,
            NOM_EVT_ADVIS_SENSOR_CHK = 6696,
            NOM_EVT_ADVIS_GAIN_DECR = 6704,
            NOM_EVT_ADVIS_GAIN_INCR = 6706,
            NOM_EVT_ADVIS_UNIT_CHK = 6710,
            NOM_EVT_APNEA = 3072,
            NOM_EVT_ECG_ASYSTOLE = 3076,
            NOM_EVT_ECG_BEAT_MISSED = 3078,
            NOM_EVT_ECG_BIGEM = 3082,
            NOM_EVT_ECG_BRADY_EXTREME = 3086,
            NOM_EVT_ECG_PACING_NON_CAPT = 3102,
            NOM_EVT_ECG_PAUSE = 3108,
            NOM_EVT_ECG_TACHY_EXTREME = 3122,
            NOM_EVT_ECG_CARD_BEAT_RATE_IRREG = 3158,
            NOM_EVT_ECG_PACER_NOT_PACING = 3182,
            NOM_EVT_ECG_SV_TACHY = 3192,
            NOM_EVT_ECG_V_P_C_RonT = 3206,
            NOM_EVT_ECG_V_P_C_MULTIFORM = 3208,
            NOM_EVT_ECG_V_P_C_PAIR = 3210,
            NOM_EVT_ECG_V_P_C_RUN = 3212,
            NOM_EVT_ECG_V_RHY = 3220,
            NOM_EVT_ECG_V_TACHY = 3224,
            NOM_EVT_ECG_V_TACHY_NON_SUST = 3226,
            NOM_EVT_ECG_V_TRIGEM = 3236,
            NOM_EVT_DESAT = 3246,
            NOM_EVT_ECG_V_P_C_RATE = 3252,
            NOM_EVT_STAT_AL_OFF = 6144,
            NOM_EVT_STAT_BATT_CHARGING = 6150,
            NOM_EVT_STAT_CALIB_MODE = 6152,
            NOM_EVT_STAT_CALIB_RUNNING = 6154,
            NOM_EVT_STAT_CALIB_INVIVO_RUNNING = 6156,
            NOM_EVT_STAT_CALIB_LIGHT_RUNNING = 6158,
            NOM_EVT_STAT_CALIB_PREINS_RUNNING = 6160,
            NOM_EVT_STAT_SELFTEST_RUNNING = 6164,
            NOM_EVT_STAT_ZERO_RUNNING = 6170,
            NOM_EVT_STAT_OPT_MOD_SENSOR_CONN = 6172,
            NOM_EVT_STAT_OPT_MOD_SENSOR_WARMING = 6174,
            NOM_EVT_STAT_SENSOR_WARMING = 6176,
            NOM_EVT_STAT_WARMING = 6178,
            NOM_EVT_STAT_ECG_AL_ALL_OFF = 6182,
            NOM_EVT_STAT_ECG_AL_SOME_OFF = 6184,
            NOM_EVT_STAT_LEARN = 6224,
            NOM_EVT_STAT_OFF = 6226,
            NOM_EVT_STAT_STANDBY = 6228,
            NOM_EVT_STAT_DISCONN = 6256,
            NOM_EVT_ADVIS_CALIB_REQD = 6662,
            NOM_EVT_ECG_V_FIB_TACHY = 61444,
            NOM_EVT_WAIT_CAL = 61678,
            NOM_EVT_ADVIS_CHANGE_SITE = 61682,
            NOM_EVT_ADVIS_CHECK_SITE_TIME = 61684,
            NOM_EVT_STAT_FW_UPDATE_IN_PROGRESS = 61688,
            NOM_EVT_EXT_DEV_AL_CODE_1 = 61690,
            NOM_EVT_EXT_DEV_AL_CODE_2 = 61692,
            NOM_EVT_EXT_DEV_AL_CODE_3 = 61694,
            NOM_EVT_EXT_DEV_AL_CODE_4 = 61696,
            NOM_EVT_EXT_DEV_AL_CODE_5 = 61698,
            NOM_EVT_EXT_DEV_AL_CODE_6 = 61700,
            NOM_EVT_EXT_DEV_AL_CODE_7 = 61702,
            NOM_EVT_EXT_DEV_AL_CODE_8 = 61704,
            NOM_EVT_EXT_DEV_AL_CODE_9 = 61706,
            NOM_EVT_EXT_DEV_AL_CODE_10 = 61708,
            NOM_EVT_EXT_DEV_AL_CODE_11 = 61710,
            NOM_EVT_EXT_DEV_AL_CODE_12 = 61712,
            NOM_EVT_EXT_DEV_AL_CODE_13 = 61714,
            NOM_EVT_EXT_DEV_AL_CODE_14 = 61716,
            NOM_EVT_EXT_DEV_AL_CODE_15 = 61718,
            NOM_EVT_EXT_DEV_AL_CODE_16 = 61720,
            NOM_EVT_EXT_DEV_AL_CODE_17 = 61722,
            NOM_EVT_EXT_DEV_AL_CODE_18 = 61724,
            NOM_EVT_EXT_DEV_AL_CODE_19 = 61726,
            NOM_EVT_EXT_DEV_AL_CODE_20 = 61728,
            NOM_EVT_EXT_DEV_AL_CODE_21 = 61730,
            NOM_EVT_EXT_DEV_AL_CODE_22 = 61732,
            NOM_EVT_EXT_DEV_AL_CODE_23 = 61734,
            NOM_EVT_EXT_DEV_AL_CODE_24 = 61736,
            NOM_EVT_EXT_DEV_AL_CODE_25 = 61738,
            NOM_EVT_EXT_DEV_AL_CODE_26 = 61740,
            NOM_EVT_EXT_DEV_AL_CODE_27 = 61742,
            NOM_EVT_EXT_DEV_AL_CODE_28 = 61744,
            NOM_EVT_EXT_DEV_AL_CODE_29 = 61746,
            NOM_EVT_EXT_DEV_AL_CODE_30 = 61748,
            NOM_EVT_EXT_DEV_AL_CODE_31 = 61750,
            NOM_EVT_EXT_DEV_AL_CODE_32 = 61752,
            NOM_EVT_EXT_DEV_AL_CODE_33 = 61754,
            NOM_EVT_ST_MULTI = 61756,
            NOM_EVT_ADVIS_BSA_REQD = 61760,
            NOM_EVT_ADVIS_PRESUMED_CVP = 61762,
            NOM_EVT_MSMT_UNSUPPORTED = 61764,
            NOM_EVT_BRADY = 61766,
            NOM_EVT_TACHY = 61768,
            NOM_EVT_ADVIS_CHANGE_SCALE = 61770,
            NOM_EVT_MSMT_RESTART = 61772,
            NOM_EVT_TOO_MANY_AGENTS = 61774,
            NOM_EVT_STAT_PULSE_SRC_RANGE_OVER = 61778,
            NOM_EVT_STAT_PRESS_SRC_RANGE_OVER = 61780,
            NOM_EVT_MUSCLE_NOISE = 61782,
            NOM_EVT_LINE_NOISE = 61784,
            NOM_EVT_IMPED_HI = 61786,
            NOM_EVT_AGENT_MIX = 61788,
            NOM_EVT_IMPEDS_HI = 61790,
            NOM_EVT_ADVIS_PWR_HI = 61792,
            NOM_EVT_ADVIS_PWR_OFF = 61794,
            NOM_EVT_ADVIS_PWR_OVER = 61796,
            NOM_EVT_ADVIS_DEACT = 61798,
            NOM_EVT_CO_WARNING = 61800,
            NOM_EVT_ADVIS_NURSE_CALL = 61802,
            NOM_EVT_COMP_MALF = 61804,
            NOM_EVT_AGENT_MEAS_MALF = 61806,
            NOM_EVT_ADVIS_WATER_TRAP_CHK = 61808,
            NOM_EVT_STAT_AGENT_CALC_RUNNING = 61810,
            NOM_EVT_ADVIS_ADAPTER_CHK = 61814,
            NOM_EVT_ADVIS_PUMP_OFF = 61816,
            NOM_EVT_ZERO_FAIL = 61818,
            NOM_EVT_ADVIS_ZERO_REQD = 61820,
            NOM_EVT_EXTR_HI = 61830,
            NOM_EVT_EXTR_LO = 61832,
            NOM_EVT_LEAD_DISCONN_YELLOW = 61833,
            NOM_EVT_LEAD_DISCONN_RED = 61834,
            NOM_EVT_CUFF_INFLAT_OVER_YELLOW = 61835,
            NOM_EVT_CUFF_INFLAT_OVER_RED = 61836,
            NOM_EVT_CUFF_NOT_DEFLATED_YELLOW = 61837,
            NOM_EVT_CUFF_NOT_DEFLATED_RED = 61838,
            NOM_EVT_ADVIS_ACTION_REQD = 61840,
            NOM_EVT_OUT_OF_AREA = 61842,
            NOM_EVT_LEADS_DISCONN = 61844,
            NOM_EVT_DEV_ASSOC_CHK = 61846,
            NOM_EVT_SYNCH_UNSUPPORTED = 61848,
            NOM_EVT_ECG_ADVIS_SRC_CHK = 61850,
            NOM_EVT_ALARM_TECH = 61852,
            NOM_EVT_ALARM_TECH_YELLOW = 61854,
            NOM_EVT_ALARM_TECH_RED = 61856,
            NOM_EVT_ALARM_MED_YELLOW_SHORT = 61858,
            NOM_EVT_ALARM_MED_YELLOW = 61860,
            NOM_EVT_ALARM_MED_RED = 61862,
            NOM_EVT_TELE_EQUIP_MALF = 61874,
            NOM_EVT_SYNCH_ERR_ECG = 61876,
            NOM_EVT_SYNCH_ERR_SPO2T = 61878,
            NOM_EVT_ADVIS_ACTION_REQD_YELLOW = 61880,
            NOM_EVT_ADVIS_NBP_SEQ_COMPLETED = 61882,
            NOM_EVT_PACER_OUTPUT_LO = 61884,
            NOM_EVT_ALARM_MORE_TECH = 61886,
            NOM_EVT_ALARM_MORE_TECH_YELLOW = 61888,
            NOM_EVT_ALARM_MORE_TECH_RED = 61890,
            NOM_EVT_ADVIS_PATIENT_CONFLICT = 61892,
            NOM_EVT_SENSOR_REPLACE = 61894,
            NOM_EVT_ECG_ATR_FIB = 61896,
            NOM_EVT_LIMITED_CONNECTIVITY = 61900,
            NOM_EVT_DISABLED = 61924,
            NOM_EVT_ECG_ABSENT = 61926,
            NOM_EVT_SRR_INTERF = 61928,
            NOM_EVT_SRR_INVALID_CHAN = 61930,
            NOM_EVT_EXT_DEV_DEMO = 62032,
            NOM_EVT_EXT_DEV_MONITORING = 62034
        }
        #endregion
        #region "Protocol Field enumerations"
        private enum MDSStatus : Int16
        {
            DISCONNECTED = 0,
            UNASSOCIATED = 1,
            OPERATING = 6
        }
        public enum Commands : Int16
        {
            CMD_EVENT_REPORT = 0,
            CMD_CONFIRMED_EVENT_REPORT = 1,
            CMD_GET = 3,
            CMD_SET = 4,
            CMD_CONFIRMED_SET = 5,
            CMD_CONFIRMED_ACTION = 7
        }
        public enum RemoteOpErrors : Int16
        {
            NO_SUCH_OBJECT_CLASS = 0,
            NO_SUCH_OBJECT_INSTANCE = 1,
            ACCESS_DENIED = 2,
            GET_LIST_ERROR = 7,
            SET_LIST_ERROR = 8,
            NO_SUCH_ACTION = 9,
            PROCESSING_FAILURE = 10,
            INVALID_ARGUMENT_VALUE = 15,
            INVALID_SCOPE = 16,
            INVALID_OBJECT_INSTANCE = 17,
            NONE = 255
        }
        public enum ErrorStatus : Int16
        {
            ATTR_ACCESS_DENIED = 2,
            ATTR_NO_SUCH_ATTRIBUTE = 5,
            ATTR_INVALID_ATTRIBUTE_VALUE = 6,
            ATTR_INVALID_OPERATION = 24,
            ATTR_INVALID_OPERATOR = 25
        }
        public enum Action : UInt16
        {
            //non-CLS, but required
            NOM_NOTI_MDS_CREAT = 3334,
            //MDS Create Notification
            NOM_ACT_POLL_MDIB_DATA = 3094,
            //Single poll
            NOM_ACT_POLL_MDIB_DATA_EXT = 61755
            //Extended Poll
            //see PollTypes
        }
        public enum PollTypes : Int16
        {
            Numerics = 6,
            //NOM_MOC_VMO_METRIC_NU
            Waves = 9,
            //NOM_MOC_VMO_METRIC_SA_RT
            Alerts = 54,
            //NOM_MOC_VMO_AL_MON  (x36)
            PatientDemographics = 42,
            //NOM_MOC_PT_DEMOG    (x2A)
            MDS = 33
            //NOM_MOC_VMS_MDS     (x21)
            //also defined in the ObjectClasses enumeration
            //partition: = &H0001
        }
        public enum RemoteOperationHeader : Int16
        {
            ROIV_APDU = 1,
            //request/command from client (PC)
            RORS_APDU = 2,
            //single frame result from monitor except error
            ROER_APDU = 3,
            //remote operation error
            ROLRS_APDU = 5
            //e.g. Single Poll Linked Result
        }
        public enum RemoteOperationLinkedResult : byte
        {
            RORLS_NONE,
            // indicates no linked message in progress -- added for this implementation
            RORLS_FIRST = 1,
            // set in the first message */
            RORLS_NOT_FIRST_NOT_LAST = 2,
            // not last
            RORLS_LAST = 3
            // last RORLSapdu, one RORSapdu
        }
        public enum PartitionIDs : Int16
        {
            NOM_PART_OBJ = 1,
            // Object Oriented Elements */
            NOM_PART_SCADA = 2,
            // Physiological Measurements */
            NOM_PART_EVT = 3,
            // Events for Alerts */
            NOM_PART_DIM = 4,
            // Units of Measurement */
            NOM_PART_PGRP = 6,
            // Identification of Parameter Groups */
            NOM_PART_INFRASTRUCT = 8,
            // Infrastructure Elements */
            NOM_PART_EMFC = 1025,
            // EMFC */
            NOM_PART_SETTINGS = 1026
            // Settings */
        }
        public enum LinkedID
        {
            RORLS_FIRST = 1,
            // set in the first message */
            RORLS_NOT_FIRST_NOT_LAST = 2,
            RORLS_LAST = 3
            // last RORLSapdu, one RORSapdu
        }
        #endregion
        #region "Natural Language enumeration"
        public enum Language : Int16
        {
            LANGUAGE_UNSPECIFIED = 0,
            ENGLISH = 1,
            GERMAN = 2,
            FRENCH = 3,
            ITALIAN = 4,
            SPANISH = 5,
            DUTCH = 6,
            SWEDISH = 7,
            FINNISH = 8,
            NORWEGIAN = 9,
            DANISH = 10,
            JAPANESE = 11,
            REP_OF_CHINA = 12,
            PEOPLE_REP_CHINA = 13,
            PORTUGUESE = 14,
            RUSSIAN = 15,
            BYELORUSSIAN = 16,
            UKRAINIAN = 17,
            CROATIAN = 18,
            SERBIAN = 19,
            MACEDONIAN = 20,
            BULGARIAN = 21,
            GREEK = 22,
            POLISH = 23,
            CZECH = 24,
            SLOVAK = 25,
            SLOVENIAN = 26,
            HUNGARIAN = 27,
            ROMANIAN = 28,
            TURKISH = 29,
            LATVIAN = 30,
            LITHUANIAN = 31,
            ESTONIAN = 32,
            KOREAN = 33
        }
        #endregion
        #region "Attributes Enumeration"
        public enum AttributeIDs : UInt16
        {
            //Device P-Alarm List
            NOM_ATTR_AL_MON_P_AL_LIST = 0x902,
            //Device T-Alarm List
            NOM_ATTR_AL_MON_T_AL_LIST = 0x904,
            //Altitude
            NOM_ATTR_ALTITUDE = 0x90c,
            //Application Area
            NOM_ATTR_AREA_APPL = 0x90d,
            //Color
            NOM_ATTR_COLOR = 0x911,
            //Device Alert Condition
            NOM_ATTR_DEV_AL_COND = 0x916,
            //Display Resolution
            NOM_ATTR_DISP_RES = 0x917,
            //Visual Grid
            NOM_ATTR_GRID_VIS_I16 = 0x91a,
            //Association Invoke Id
            NOM_ATTR_ID_ASSOC_NO = 0x91d,
            //Bed Label
            NOM_ATTR_ID_BED_LABEL = 0x91e,
            //Object Handle
            NOM_ATTR_ID_HANDLE = 0x921,
            //Label
            NOM_ATTR_ID_LABEL = 0x924,
            //Label String
            NOM_ATTR_ID_LABEL_STRING = 0x927,
            //System Model
            NOM_ATTR_ID_MODEL = 0x928,
            //Product Specification
            NOM_ATTR_ID_PROD_SPECN = 0x92d,
            //Object Type
            NOM_ATTR_ID_TYPE = 0x92f,
            //Line Frequency
            NOM_ATTR_LINE_FREQ = 0x935,
            //System Localization
            NOM_ATTR_LOCALIZN = 0x937,
            //Metric Info Label
            NOM_ATTR_METRIC_INFO_LABEL = 0x93c,
            //Metric Info Label String
            NOM_ATTR_METRIC_INFO_LABEL_STR = 0x93d,
            //Metric Specification
            NOM_ATTR_METRIC_SPECN = 0x93f,
            //Metric State
            NOM_ATTR_METRIC_STAT = 0x940,
            //Measure Mode
            NOM_ATTR_MODE_MSMT = 0x945,
            //Operating Mode
            NOM_ATTR_MODE_OP = 0x946,
            //Nomenclature Version
            NOM_ATTR_NOM_VERS = 0x948,
            //Compound Numeric Observed Value
            NOM_ATTR_NU_CMPD_VAL_OBS = 0x94b,
            //Numeric Observed Value
            NOM_ATTR_NU_VAL_OBS = 0x950,
            //Patient BSA
            NOM_ATTR_PT_BSA = 0x956,
            //Pat Demo State
            NOM_ATTR_PT_DEMOG_ST = 0x957,
            //Patient Date of Birth
            NOM_ATTR_PT_DOB = 0x958,
            //Patient ID
            NOM_ATTR_PT_ID = 0x95a,
            //Family Name
            NOM_ATTR_PT_NAME_FAMILY = 0x95c,
            //Given Name
            NOM_ATTR_PT_NAME_GIVEN = 0x95d,
            //Patient Sex
            NOM_ATTR_PT_SEX = 0x961,
            //Patient Type
            NOM_ATTR_PT_TYPE = 0x962,
            //Sample Array Calibration Specification
            NOM_ATTR_SA_CALIB_I16 = 0x964,
            //Compound Sample Array Observed Value
            NOM_ATTR_SA_CMPD_VAL_OBS = 0x967,
            //Sample Array Physiological Range
            NOM_ATTR_SA_RANGE_PHYS_I16 = 0x96a,
            //Sample Array Specification
            NOM_ATTR_SA_SPECN = 0x96d,
            //Sample Array Observed Value
            NOM_ATTR_SA_VAL_OBS = 0x96e,
            //Scale and Range Specification
            NOM_ATTR_SCALE_SPECN_I16 = 0x96f,
            //Safety Standard
            NOM_ATTR_STD_SAFETY = 0x982,
            //System ID
            NOM_ATTR_SYS_ID = 0x984,
            //System Specification
            NOM_ATTR_SYS_SPECN = 0x985,
            //System Type
            NOM_ATTR_SYS_TYPE = 0x986,
            //Date and Time
            NOM_ATTR_TIME_ABS = 0x987,
            //Sample Period
            NOM_ATTR_TIME_PD_SAMP = 0x98d,
            //Relative Time
            NOM_ATTR_TIME_REL = 0x98f,
            //Absolute Time Stamp
            NOM_ATTR_TIME_STAMP_ABS = 0x990,
            //Relative Time Stamp
            NOM_ATTR_TIME_STAMP_REL = 0x991,
            //Unit Code
            NOM_ATTR_UNIT_CODE = 0x996,
            //Enumeration Observed Value
            NOM_ATTR_VAL_ENUM_OBS = 0x99e,
            //MDS Status
            NOM_ATTR_VMS_MDS_STAT = 0x9a7,
            //Patient Age
            NOM_ATTR_PT_AGE = 0x9d8,
            //Patient Height
            NOM_ATTR_PT_HEIGHT = 0x9dc,
            //Patient Weight
            NOM_ATTR_PT_WEIGHT = 0x9df,
            //Sample Array Fixed Values Specification
            NOM_ATTR_SA_FIXED_VAL_SPECN = 0xa16,
            //Patient Paced Mode
            NOM_ATTR_PT_PACED_MODE = 0xa1e,
            //Internal Patient ID
            NOM_ATTR_PT_ID_INT = 0xf001,
            //Private Attribute
            NOM_SAT_O2_TONE_FREQ = 0xf008,
            //Private Attribute
            NOM_ATTR_CMPD_REF_LIST = 0xf009,
            //IP Address Information
            NOM_ATTR_NET_ADDR_INFO = 0xf100,
            //Protocol Support
            NOM_ATTR_PCOL_SUPPORT = 0xf101,
            //Notes1
            NOM_ATTR_PT_NOTES1 = 0xf129,
            //Notes2
            NOM_ATTR_PT_NOTES2 = 0xf12a,
            //Time for Periodic Polling
            NOM_ATTR_TIME_PD_POLL = 0xf13e,
            //Patient BSA Formula
            NOM_ATTR_PT_BSA_FORMULA = 0xf1ec,
            //Mds General System Info
            NOM_ATTR_MDS_GEN_INFO = 0xf1fa,
            //no of prioritized objects for poll request
            NOM_ATTR_POLL_OBJ_PRIO_NUM = 0xf228,
            //Numeric Object Priority List
            NOM_ATTR_POLL_NU_PRIO_LIST = 0xf239,
            //Wave Object Priority List
            NOM_ATTR_POLL_RTSA_PRIO_LIST = 0xf23a,
            //Metric Modality
            NOM_ATTR_METRIC_MODALITY = 0xf294,
            //The attributes are arranged in the following attribute groups:
            //Alert Monitor Group
            NOM_ATTR_GRP_AL_MON = 0x801,
            //Metric Observed Value Group
            NOM_ATTR_GRP_METRIC_VAL_OBS = 0x803,
            //Patient Demographics Attribute Group
            NOM_ATTR_GRP_PT_DEMOG = 0x807,
            //System Application Attribute Group
            NOM_ATTR_GRP_SYS_APPL = 0x80a,
            //System Identification Attribute Group
            NOM_ATTR_GRP_SYS_ID = 0x80b,
            //System Production Attribute Group
            NOM_ATTR_GRP_SYS_PROD = 0x80c,
            //VMO Dynamic Attribute Group
            NOM_ATTR_GRP_VMO_DYN = 0x810,
            //VMO Static Attribute Group
            NOM_ATTR_GRP_VMO_STATIC = 0x811
        }
        #endregion
        #region "Monitor Alerts enumerations"
        public enum AlertState : UInt16
        {
            //non-CLS, but required
            AL_INHIBITED = 0x8000,
            AL_SUSPENDED = 0x4000,
            AL_LATCHED = 0x2000,
            AL_SILENCED_RESET = 0x1000,
            AL_DEV_IN_TEST_MODE = 0x400,
            AL_DEV_IN_STANDBY = 0x200,
            AL_DEV_IN_DEMO_MODE = 0x100,
            AL_NEW_ALERT = 0x8
        }
        public enum MonitorAlertType : Int16
        {
            NO_ALERT = 0,
            LOW_PRI_T_AL = 1,
            MED_PRI_T_AL = 2,
            HI_PRI_T_AL = 4,
            LOW_PRI_P_AL = 256,
            MED_PRI_P_AL = 512,
            HI_PRI_P_AL = 1024
        }
        public enum AlarmInfo : Int16
        {
            GEN_ALMON_INFO = 513,
            STR_ALMON_INFO = 516
        }
        #endregion
        #region "Physiological ID enumerations"
        public enum ECG : UInt16
        {
            //not CLS
            NOM_ECG_ELEC_POTL = 0x100,
            NOM_ECG_ELEC_POTL_I = 0x101,
            NOM_ECG_ELEC_POTL_II = 0x102,
            NOM_ECG_ELEC_POTL_III = 0x13d,
            NOM_ECG_ELEC_POTL_AVR = 0x13e
        }
        #endregion
        #region "Units Enumeration"
        private enum UnitCode : UInt16
        {
            //non-CLS
            NOM_DIM_NOS = 0,
            NOM_DIM_DIV = 2,
            //( no dimension )
            NOM_DIM_DIMLESS = 512,
            // % ( percentage )
            NOM_DIM_PERCENT = 544,
            //ppth ( parts per thousand )
            NOM_DIM_PARTS_PER_THOUSAND = 576,
            // ppm ( parts per million )
            NOM_DIM_PARTS_PER_MILLION = 608,
            // mol/mol ( mole per mole )
            NOM_DIM_X_MOLE_PER_MOLE = 864,
            // ppb ( parts per billion )
            NOM_DIM_PARTS_PER_BILLION = 672,
            //ppt ( parts per trillion )
            NOM_DIM_PARTS_PER_TRILLION = 704,
            // pH ( pH )
            NOM_DIM_PH = 992,
            //drop ( vital signs count drop )
            NOM_DIM_DROP = 1024,
            // rbc ( vital signs count red blood cells )
            NOM_DIM_RBC = 1056,
            //beat ( vital signs count beat )
            NOM_DIM_BEAT = 1088,
            //breath ( vital signs count breath )
            NOM_DIM_BREATH = 1120,
            //cell ( vital signs count cells )
            NOM_DIM_CELL = 1152,
            //cough ( vital signs count cough )
            NOM_DIM_COUGH = 1184,
            //sigh ( vital signs count sigh )
            NOM_DIM_SIGH = 1216,
            //%PCV ( percent of packed cell volume )
            NOM_DIM_PCT_PCV = 1248,
            //m ( meter )
            NOM_DIM_X_M = 1280,
            //cm ( centimeter )
            NOM_DIM_CENTI_M = 1297,
            // mm ( millimeter )
            NOM_DIM_MILLI_M = 1298,
            // μm ( micro-meter )
            NOM_DIM_MICRO_M = 1299,
            // in ( inch )
            NOM_DIM_X_INCH = 1376,
            //ml/m2 ( used e.g. for SI and ITBVI )
            NOM_DIM_MILLI_L_PER_M_SQ = 1426,
            ///m ( per meter )
            NOM_DIM_PER_X_M = 1440,
            ///mm ( per millimeter )
            NOM_DIM_PER_MILLI_M = 1458,
            //m2 ( used e.g. for BSA calculation )
            NOM_DIM_SQ_X_M = 1472,
            //in2 ( used e.g. for BSA calculation )
            NOM_DIM_SQ_X_INCH = 1504,
            //m3 ( cubic meter )
            NOM_DIM_CUBIC_X_M = 1568,
            //cm3 ( cubic centimeter )
            NOM_DIM_CUBIC_CENTI_M = 1585,
            //l ( liter )
            NOM_DIM_X_L = 1600,
            //ml ( milli-liters used e.g. for EVLW ITBV SV )
            NOM_DIM_MILLI_L = 1618,
            //ml/breath ( milli-liter per breath )
            NOM_DIM_MILLI_L_PER_BREATH = 1650,
            ///cm3 ( per cubic centimeter ) 
            NOM_DIM_PER_CUBIC_CENTI_M = 1681,
            ///l ( per liter )
            NOM_DIM_PER_X_L = 1696,
            //1/nl ( per nano-liter )
            NOM_DIM_PER_NANO_LITER = 1716,
            //g ( gram )
            NOM_DIM_X_G = 1728,
            //kg ( kilo-gram )
            NOM_DIM_KILO_G = 1731,
            // mg ( milli-gram )
            NOM_DIM_MILLI_G = 1746,
            //μg ( micro-gram )
            NOM_DIM_MICRO_G = 1747,
            // ng ( nono-gram )
            NOM_DIM_NANO_G = 1748,
            //lb ( pound )
            NOM_DIM_X_LB = 1760,
            // oz ( ounce )
            NOM_DIM_X_OZ = 1792,
            // /g ( per gram )
            NOM_DIM_PER_X_G = 1824,
            //g-m ( used e.g. for LVSW RVSW )
            NOM_DIM_X_G_M = 1856,
            // kg-m ( used e.g. for RCW LCW )
            NOM_DIM_KILO_G_M = 1859,
            //g-m/m2 ( used e.g. for LVSWI and RVSWI )
            NOM_DIM_X_G_M_PER_M_SQ = 1888,
            //kg-m/m2 ( used e.g. for LCWI and RCWI )
            NOM_DIM_KILO_G_M_PER_M_SQ = 1891,
            //kg-m2 ( gram meter squared )
            NOM_DIM_KILO_G_M_SQ = 1923,
            // kg/m2 ( kilo-gram per square meter )
            NOM_DIM_KG_PER_M_SQ = 1955,
            //kg/m3 ( kilo-gram per cubic meter )
            NOM_DIM_KILO_G_PER_M_CUBE = 1987,
            // g/cm3 ( gram per cubic meter )
            NOM_DIM_X_G_PER_CM_CUBE = 2016,
            // mg/cm3 ( milli-gram per cubic centimeter )
            NOM_DIM_MILLI_G_PER_CM_CUBE = 2034,
            //μg/cm3 ( micro-gram per cubic centimeter )
            NOM_DIM_MICRO_G_PER_CM_CUBE = 2035,
            //ng/cm3 ( nano-gram per cubic centimeter )
            NOM_DIM_NANO_G_PER_CM_CUBE = 2036,
            //g/l ( gram per liter )
            NOM_DIM_X_G_PER_L = 2048,
            //g/dl ( used e.g. for Hb )
            NOM_DIM_X_G_PER_DL = 2112,
            //mg/dl ( milli-gram per deciliter )
            NOM_DIM_MILLI_G_PER_DL = 2130,
            //g/ml ( gram per milli-liter )
            NOM_DIM_X_G_PER_ML = 2144,
            //mg/ml ( milli-gram per milli-liter )
            NOM_DIM_MILLI_G_PER_ML = 2162,
            // μg/ml ( micro-gram per milli-liter )
            NOM_DIM_MICRO_G_PER_ML = 2163,
            //ng/ml ( nano-gram per milli-liter )
            NOM_DIM_NANO_G_PER_ML = 2164,
            //sec ( seconds )
            NOM_DIM_SEC = 2176,
            //msec ( milli-seconds )
            NOM_DIM_MILLI_SEC = 2194,
            //μsec ( micro-seconds )
            NOM_DIM_MICRO_SEC = 2195,
            // min ( minutes )
            NOM_DIM_MIN = 2208,
            //hrs ( hours )
            NOM_DIM_HR = 2240,
            //days ( days )
            NOM_DIM_DAY = 2272,
            //weeks ( weeks )
            NOM_DIM_WEEKS = 2304,
            // months ( months )
            NOM_DIM_MON = 2336,
            // years ( years )
            NOM_DIM_YR = 2368,
            // TOD ( time of day )
            NOM_DIM_TOD = 2400,
            //date ( date )
            NOM_DIM_DATE = 2432,
            // /sec ( per second )
            NOM_DIM_PER_X_SEC = 2464,
            // Hz ( hertz )
            NOM_DIM_HZ = 2496,
            // /min ( per minute used e.g. for the PVC count numerical value )
            NOM_DIM_PER_MIN = 2528,
            ///hour ( per hour )
            NOM_DIM_PER_HR = 2560,
            // /day ( per day )
            NOM_DIM_PER_DAY = 2592,
            // /week ( per week )
            NOM_DIM_PER_WK = 2624,
            // /month ( per month )
            NOM_DIM_PER_MO = 2656,
            ///year ( per year )
            NOM_DIM_PER_YR = 2688,
            //bpm ( beats per minute used e.g. for HR/PULSE )
            NOM_DIM_BEAT_PER_MIN = 2720,
            //puls/min ( puls per minute )
            NOM_DIM_PULS_PER_MIN = 2752,
            // rpm ( respiration breathes per minute )
            NOM_DIM_RESP_PER_MIN = 2784,
            //m/sec ( meter per second )
            NOM_DIM_X_M_PER_SEC = 2816,
            // mm/sec ( speed for recordings )
            NOM_DIM_MILLI_M_PER_SEC = 2834,
            // l/min/m2 ( used for CI )
            NOM_DIM_X_L_PER_MIN_PER_M_SQ = 2848,
            // ml/min/m2 ( used for DO2I VO2I O2AVI )
            NOM_DIM_MILLI_L_PER_MIN_PER_M_SQ = 2866,
            // m2/sec ( square meter per second )
            NOM_DIM_SQ_X_M_PER_SEC = 2880,
            //cm2/sec ( square centimeter per second )
            NOM_DIM_SQ_CENTI_M_PER_SEC = 2897,
            // m3/sec ( cubic meter per second )
            NOM_DIM_CUBIC_X_M_PER_SEC = 2912,
            //cm3/sec ( cubic centimeter per second )
            NOM_DIM_CUBIC_CENTI_M_PER_SEC = 2929,
            //l/sec ( liter per second )
            NOM_DIM_X_L_PER_SEC = 3040,
            //l/min ( liter per minutes )
            NOM_DIM_X_L_PER_MIN = 3072,
            // dl/min ( deciliter per second )
            NOM_DIM_DECI_L_PER_MIN = 3088,
            // ml/min ( used for DO2 VO2 ALVENT )
            NOM_DIM_MILLI_L_PER_MIN = 3090,
            // l/hour ( liter per hour )
            NOM_DIM_X_L_PER_HR = 3104,
            //ml/hour ( milli-liter per hour )
            NOM_DIM_MILLI_L_PER_HR = 3122,
            //l/day ( liter per day )
            NOM_DIM_X_L_PER_DAY = 3136,
            // ml/day ( milli-liter per day )
            NOM_DIM_MILLI_L_PER_DAY = 3154,
            // ml/kg ( used e.g. for EVLWI )
            NOM_DIM_MILLI_L_PER_KG = 3186,
            //kg/sec ( kilo-gram per second )
            NOM_DIM_KILO_G_PER_SEC = 3299,
            // g/min ( gram per minute )
            NOM_DIM_X_G_PER_MIN = 3328,
            //kg/min ( kilo-gram per minute )
            NOM_DIM_KILO_G_PER_MIN = 3331,
            // mg/min ( milli-gram per minute )
            NOM_DIM_MILLI_G_PER_MIN = 3346,
            //μg/min ( micro-gram per minute )
            NOM_DIM_MICRO_G_PER_MIN = 3347,
            //ng/min ( nano-gram per minute )
            NOM_DIM_NANO_G_PER_MIN = 3348,
            //g/hour ( gram per hour )
            NOM_DIM_X_G_PER_HR = 3360,
            //kg/hour ( kilo-gram per hour )
            NOM_DIM_KILO_G_PER_HR = 3363,
            // mg/hour ( milli-gram per hour )
            NOM_DIM_MILLI_G_PER_HR = 3378,
            //μg/hour ( micro-gram per hour )
            NOM_DIM_MICRO_G_PER_HR = 3379,
            //ng/hr ( nano-gram per hour )
            NOM_DIM_NANO_G_PER_HR = 3380,
            //kg/day ( kilo-gram per day )
            NOM_DIM_KILO_G_PER_DAY = 3395,
            //g/kg/min ( gram per kilo-gram per minute )
            NOM_DIM_X_G_PER_KG_PER_MIN = 3456,
            // mg/kg/min ( milli-gram per kilo-gram per minute )
            NOM_DIM_MILLI_G_PER_KG_PER_MIN = 3474,
            // μg/kg/min ( micro-gram per kilo-gram per minute )
            NOM_DIM_MICRO_G_PER_KG_PER_MIN = 3475,
            //ng/kg/min ( nano-gram per kilo-gram per minute )
            NOM_DIM_NANO_G_PER_KG_PER_MIN = 3476,
            // g/kg/hour ( gram per kilo-gram per hour )
            NOM_DIM_X_G_PER_KG_PER_HR = 3488,
            // mg/kg/hour ( mili-gram per kilo-gram per hour )
            NOM_DIM_MILLI_G_PER_KG_PER_HR = 3506,
            //μg/kg/hour ( micro-gram per kilo-gram per hour )
            NOM_DIM_MICRO_G_PER_KG_PER_HR = 3507,
            // ng/kg/hour ( nano-gram per kilo-gram per hour )
            NOM_DIM_NANO_G_PER_KG_PER_HR = 3508,
            //kg/l/sec ( kilo-gram per liter per second )
            NOM_DIM_KILO_G_PER_L_SEC = 3555,
            // kg/m/sec ( kilo-gram per meter per second )
            NOM_DIM_KILO_G_PER_M_PER_SEC = 3683,
            // kg-m/sec ( kilo-gram meter per second )
            NOM_DIM_KILO_G_M_PER_SEC = 3715,
            //N-s ( newton seconds )
            NOM_DIM_X_NEWTON_SEC = 3744,
            //N ( newton )
            NOM_DIM_X_NEWTON = 3776,
            //Pa ( pascal )
            NOM_DIM_X_PASCAL = 3840,
            //hPa ( hekto-pascal )
            NOM_DIM_HECTO_PASCAL = 3842,
            //kPa ( kilo-pascal )
            NOM_DIM_KILO_PASCAL = 3843,
            //mmHg ( mm mercury )
            NOM_DIM_MMHG = 3872,
            // cmH2O ( centimeter H20 )
            NOM_DIM_CM_H2O = 3904,
            //mBar ( milli-bar )
            NOM_DIM_MILLI_BAR = 3954,
            //J ( Joules )
            NOM_DIM_X_JOULES = 3968,
            //eV ( electronvolts )
            NOM_DIM_EVOLT = 4000,
            //W ( watt )
            NOM_DIM_X_WATT = 4032,
            //mW ( milli-watt )
            NOM_DIM_MILLI_WATT = 4050,
            //nW ( nano-watt )
            NOM_DIM_NANO_WATT = 4052,
            //pW ( pico-watt )
            NOM_DIM_PICO_WATT = 4053,
            //Dyn-sec/cm^5 ( dyne second per cm^5 )
            NOM_DIM_X_DYNE_PER_SEC_PER_CM5 = 4128,
            //A ( ampere )
            NOM_DIM_X_AMPS = 4160,
            //mA ( milli-ampereused e.g. for the battery indications )
            NOM_DIM_MILLI_AMPS = 4178,
            // C ( coulomb )
            NOM_DIM_X_COULOMB = 4192,
            // μC ( micro-coulomb )
            NOM_DIM_MICRO_COULOMB = 4211,
            //V ( volts )
            NOM_DIM_X_VOLT = 4256,
            //mV ( milli-volt )
            NOM_DIM_MILLI_VOLT = 4274,
            //μV ( micro-volt )
            NOM_DIM_MICRO_VOLT = 4275,
            // Ohm ( Ohm )
            NOM_DIM_X_OHM = 4288,
            //kOhm(kilo - ohm)
            NOM_DIM_OHM_K = 4291,
            //F(farad)
            NOM_DIM_X_FARAD = 4352,
            //°K ( kelvin )
            NOM_DIM_KELVIN = 4384,
            //°F ( degree-fahrenheit )
            NOM_DIM_FAHR = 4416,
            //cd ( candela )
            NOM_DIM_X_CANDELA = 4480,
            // mOsm ( milli-osmole )
            NOM_DIM_MILLI_OSM = 4530,
            //mol ( mole )
            NOM_DIM_X_MOLE = 4544,
            //mmol( milli-mole )
            NOM_DIM_MILLI_MOLE = 4562,
            //mEq ( milli-equivalents )
            NOM_DIM_MILLI_EQUIV = 4594,
            //mOsm/l ( milli-osmole per liter )
            NOM_DIM_MILLI_OSM_PER_L = 4626,
            //mmol/l ( used for HB )
            NOM_DIM_MILLI_MOLE_PER_L = 4722,
            //μmol/l ( micro-mol per liter )
            NOM_DIM_MICRO_MOLE_PER_L = 4723,
            //mEq/l ( milli-equivalents per liter )
            NOM_DIM_MILLI_EQUIV_PER_L = 4850,
            // mEq/day ( milli-equivalents per day )
            NOM_DIM_MILLI_EQUIV_PER_DAY = 5202,
            // i.u. ( international unit )
            NOM_DIM_X_INTL_UNIT = 5472,
            // mi.u. ( mili-international unit )
            NOM_DIM_MILLI_INTL_UNIT = 5490,
            // i.u./cm3 ( international unit per cubic centimeter )
            NOM_DIM_X_INTL_UNIT_PER_CM_CUBE = 5504,
            // mi.u./cm3 ( mili-international unit per cubic centimeter )
            NOM_DIM_MILLI_INTL_UNIT_PER_CM_CUBE = 5522,
            //i.u./ml ( international unit per milli-liter )
            NOM_DIM_X_INTL_UNIT_PER_ML = 5600,
            // i.u./min ( international unit per minute )
            NOM_DIM_X_INTL_UNIT_PER_MIN = 5664,
            // mi.u./ml ( milli-international unit per milli-liter )
            NOM_DIM_MILLI_INTL_UNIT_PER_ML = 5618,
            // mi.u./min ( milli-international unit per minute )
            NOM_DIM_MILLI_INTL_UNIT_PER_MIN = 5682,
            //i.u./hour ( international unit per hour )
            NOM_DIM_X_INTL_UNIT_PER_HR = 5696,
            // mi.u./hour ( milli-international unit per hour )
            NOM_DIM_MILLI_INTL_UNIT_PER_HR = 5714,
            //i.u./kg/min ( international unit per kilo-gram per minute )
            NOM_DIM_X_INTL_UNIT_PER_KG_PER_MIN = 5792,
            //mi.u./kg/min ( milli-international unit per kilo-gram per minute )
            NOM_DIM_MILLI_INTL_UNIT_PER_KG_PER_MIN = 5810,
            //i.u./kg/hour ( international unit per kilo-gram per hour )
            NOM_DIM_X_INTL_UNIT_PER_KG_PER_HR = 5824,
            // mi.u./kg/hour ( milli-international unit per kilo-gram per hour )
            NOM_DIM_MILLI_INTL_UNIT_PER_KG_PER_HR = 5842,
            //ml/cmH2O ( milli-liter per centimeter H2O )
            NOM_DIM_MILLI_L_PER_CM_H2O = 5906,
            //cmH2O/l/sec ( centimeter H2O per second )
            NOM_DIM_CM_H2O_PER_L_PER_SEC = 5920,
            //ml2/sec ( milli-liter per second )
            NOM_DIM_MILLI_L_SQ_PER_SEC = 5970,
            //cmH2O/% ( centimeter H2O per percent )
            NOM_DIM_CM_H2O_PER_PERCENT = 5984,
            //DS*m2/cm5 ( used for SVRI and PVRI )
            NOM_DIM_DYNE_SEC_PER_M_SQ_PER_CM_5 = 6016,
            // °C ( degree-celsius )
            NOM_DIM_DEGC = 6048,
            // cmH2O/l ( centimeter H2O per liter )
            NOM_DIM_CM_H2O_PER_L = 6144,
            // mmHg/% ( milli-meter mercury per percent )
            NOM_DIM_MM_HG_PER_PERCENT = 6176,
            // kPa/% ( kilo-pascal per percent )
            NOM_DIM_KILO_PA_PER_PERCENT = 6211,
            //l/mmHg (liter per mmHg)
            NOM_DIM_X_L_PER_MM_HG = 6272,
            // ml/mmHg (milli-liter per milli-meter Hg)
            NOM_DIM_MILLI_L_PER_MM_HG = 6290,
            //mAh ( milli-ampere per hour used e.g. for the battery indications )
            NOM_DIM_MILLI_AMP_HR = 6098,
            //ml/dl ( used for CaO2 CvO2 Ca-vO2 )
            NOM_DIM_MILLI_L_PER_DL = 6418,
            // dB ( decibel )
            NOM_DIM_DECIBEL = 6432,
            // g/mg ( gram per milli-gram )
            NOM_DIM_X_G_PER_MILLI_G = 6464,
            //mg/mg ( milli-gram per milli-gram )
            NOM_DIM_MILLI_G_PER_MILLI_G = 6482,
            //bpm/l ( beats per minute per liter )
            NOM_DIM_BEAT_PER_MIN_PER_X_L = 6496,
            //bpm/ml ( beats per minute per milli-liter )
            NOM_DIM_BEAT_PER_MIN_PER_MILLI_L = 6514,
            // 1/(min*l) ( per minute per liter )
            NOM_DIM_PER_X_L_PER_MIN = 6528,
            //m/min ( meter per minute )
            NOM_DIM_X_M_PER_MIN = 6560,
            // cm/min ( speed for recordings )
            NOM_DIM_CENTI_M_PER_MIN = 6577,
            //pg/ml ( pico-gram per milli-liter )
            NOM_DIM_PICO_G_PER_ML = 2165,
            //ug/l ( micro-gram per liter )
            NOM_DIM_MICRO_G_PER_L = 2067,
            //ng/l ( nano-gram per liter )
            NOM_DIM_NANO_G_PER_L = 2068,
            ///mm3 ( per cubic millimeter )
            NOM_DIM_PER_CUBIC_MILLI_M = 1682,
            //mm3 ( cubic milli-meter )
            NOM_DIM_CUBIC_MILLI_M = 1586,
            //u/l ( intl. units per liter )
            NOM_DIM_X_INTL_UNIT_PER_L = 5568,
            // /l ( 10^6 intl. units per liter )
            NOM_DIM_MEGA_INTL_UNIT_PER_L = 5573,
            // mol/kg ( mole per kilo-gram )
            NOM_DIM_MILLI_MOL_PER_KG = 4946,
            // mcg/dl ( micro-gram per deci-liter )
            NOM_DIM_MICRO_G_PER_DL = 2131,
            //mg/l ( milli-gram per liter )
            NOM_DIM_MILLI_G_PER_L = 2066,
            // / ul(micro - liter)
            NOM_DIM_PER_MICRO_L = 1715,
            //complx ( - )
            NOM_DIM_COMPLEX = 61440,
            //count ( count as a dimension )
            NOM_DIM_COUNT = 61441,
            // part ( part )
            NOM_DIM_PART = 61442,
            // puls ( puls )
            NOM_DIM_PULS = 61443,
            // μV p-p ( micro-volt peak to peak )
            NOM_DIM_UV_PP = 61444,
            // μV2 ( micor-volt square )
            NOM_DIM_UV_SQ = 61445,
            //lumen ( lumen )
            NOM_DIM_LUMEN = 61447,
            //lb/in2 ( pound per square inch )
            NOM_DIM_LB_PER_INCH_SQ = 61448,
            // mmHg/s ( milli-meter mercury per second )
            NOM_DIM_MM_HG_PER_SEC = 61449,
            // ml/s ( milli-liter per second )
            NOM_DIM_ML_PER_SEC = 61450,
            //bpm/ml ( beat per minute per milli-liter )
            NOM_DIM_BEAT_PER_MIN_PER_ML_C = 61451,
            //J/day ( joule per day )
            NOM_DIM_X_JOULE_PER_DAY = 61536,
            //kJ/day ( kilo joule per day )
            NOM_DIM_KILO_JOULE_PER_DAY = 61539,
            //MJ/day ( mega joule per day )
            NOM_DIM_MEGA_JOULE_PER_DAY = 61540,
            // cal ( calories )
            NOM_DIM_X_CALORIE = 61568,
            //kcal ( kilo calories )
            NOM_DIM_KILO_CALORIE = 61571,
            // 10**6 cal ( million calories )
            NOM_DIM_MEGA_CALORIE = 61572,
            //cal/day ( calories per day )
            NOM_DIM_X_CALORIE_PER_DAY = 61600,
            // kcal/day ( kilo-calories per day )
            NOM_DIM_KILO_CALORIE_PER_DAY = 61603,
            //Mcal/day ( mega calories per day )
            NOM_DIM_MEGA_CALORIE_PER_DAY = 61604,
            //cal/ml ( calories per milli-liter )
            NOM_DIM_X_CALORIE_PER_ML = 61632,
            // kcal/ml ( kilo calories per ml )
            NOM_DIM_KILO_CALORIE_PER_ML = 61635,
            // J/ml ( Joule per milli-liter )
            NOM_DIM_X_JOULE_PER_ML = 61664,
            // kJ/ml ( kilo-joules per milli-liter )
            NOM_DIM_KILO_JOULE_PER_ML = 61667,
            //RPM ( revolutions per minute )
            NOM_DIM_X_REV_PER_MIN = 61696,
            // l/(mn*l*kg) ( per minute per liter per kilo )
            NOM_DIM_PER_L_PER_MIN_PER_KG = 61728,
            //l/mbar ( liter per milli-bar )
            NOM_DIM_X_L_PER_MILLI_BAR = 61760,
            //ml/mbar ( milli-liter per milli-bar )
            NOM_DIM_MILLI_L_PER_MILLI_BAR = 61778,
            //l/kg/hr ( liter per kilo-gram per hour )
            NOM_DIM_X_L_PER_KG_PER_HR = 6179,
            //2 ml/kg/hr ( milli-liter per kilogram per hour )
            NOM_DIM_MILLI_L_PER_KG_PER_HR = 61810,
            // bar/l/s ( bar per liter per sec )
            NOM_DIM_X_BAR_PER_LITER_PER_SEC = 61824,
            //mbar/l/s ( milli-bar per liter per sec )
            NOM_DIM_MILLI_BAR_PER_LITER_PER_SEC = 61842,
            //bar/l ( bar per liter )
            NOM_DIM_X_BAR_PER_LITER = 61856,
            //mbar/l= ( bar per liter )
            NOM_DIM_MILLI_BAR_PER_LITER = 61874,
            //V/mV ( volt per milli-volt )
            NOM_DIM_VOLT_PER_MILLI_VOLT = 61888,
            // cmH2O/uV ( cm H2O per micro-volt )
            NOM_DIM_CM_H2O_PER_MICRO_VOLT = 61920,
            // J/l ( joule per liter )
            NOM_DIM_X_JOULE_PER_LITER = 61952,
            //l/bar ( liter per bar )
            NOM_DIM_X_L_PER_BAR = 61984,
            //m/mV ( meter per milli-volt )
            NOM_DIM_X_M_PER_MILLI_VOLT = 62016,
            // mm/mV ( milli-meter per milli-volt )
            NOM_DIM_MILLI_M_PER_MILLI_VOLT = 62034,
            //l/min/kg ( liter per minute per kilo-gram )
            NOM_DIM_X_L_PER_MIN_PER_KG = 62048,
            // ml/min/kg ( milli-liter per minute per kilo-gram )
            NOM_DIM_MILLI_L_PER_MIN_PER_KG = 62066,
            //Pa/l/s ( pascal per liter per sec )
            NOM_DIM_X_PASCAL_PER_L_PER_SEC = 62080,
            //hPa/l/s ( hPa per liter per sec )
            NOM_DIM_HECTO_PASCAL_PER_L_PER_SEC = 62082,
            //kPa/l/s ( kPa per liter per sec )
            NOM_DIM_KILO_PASCAL_PER_L_PER_SEC = 62083,
            // ml/Pa ( milli-liter per pascal )
            NOM_DIM_MILLI_L_PER_X_PASCAL = 62112,
            //ml/hPa ( milli-liter per hecto-pascal )
            NOM_DIM_MILLI_L_PER_HECTO_PASCAL = 62114,
            //ml/kPa ( milli-liter per kilo-pascal )
            NOM_DIM_MILLI_L_PER_KILO_PASCAL = 62115,
            //mmHg/l/s ( mm )
            NOM_DIM_MM_HG_PER_X_L_PER_SEC = 62144
        }
        #endregion
        #region "Metric Category enumeration"
        private enum MetricCategory : Int16
        {
            MCAT_UNSPEC = 0,
            //not specified
            AUTO_MEASUREMENT = 1,
            //automatic measurement
            MANUAL_MEASUREMENT = 2,
            //manual measurement
            AUTO_SETTING = 3,
            //automatic setting
            MANUAL_SETTING = 4,
            //manual setting
            AUTO_CALCULATION = 5,
            //automatic calculation
            MANUAL_CALCULATION = 6,
            //manual calculation
            MULTI_DYNAMIC_CAPABILITIES = 50,
            // this measurement may change its category during
            // operation or may be used in various modes
            AUTO_ADJUST_PAT_TEMP = 128,
            //measurement is automatically adjusted for patient temperature
            MANUAL_ADJUST_PAT_TEMP = 129,
            //measurement manually adjusted for patient temperature
            AUTO_ALARM_LIMIT_SETTING = 130
            //this is not a measurement, but an alarm limit setting
        }
        #endregion
        #region "Operating Mode property and enumerations"
        private enum OperatingMode : UInt16
        {
            //non CLS, but required for this bit field
            OPMODE_UNSPEC = 0x8000,
            MONITORING = 0x4000,
            DEMO = 0x2000,
            SERVICE = 0x1000,
            OPMODE_STANDBY = 0x2,
            CONFIG = 0x1
        }
        public enum PublicOperatingMode : int
        {
            //public enumeration of above
            OPMODE_UNSPEC = 0x8000,
            MONITORING = 0x4000,
            DEMO = 0x2000,
            SERVICE = 0x1000,
            OPMODE_STANDBY = 0x2,
            CONFIG = 0x1
        }
        #endregion
        #region "Application Area property and enumeration"
        public enum ApplicationArea : Int16
        {
            AREA_UNSPEC = 0,
            AREA_OPERATING_ROOM = 1,
            AREA_INTENSIVE_CARE = 2,
            AREA_NEONATAL_INTENSIVE_CARE = 3,
            AREA_CARDIOLOGY_CARE = 4
        }
        #endregion
        #region "Misc Protocol structure enumerations"
        private enum LineFrequency : Int16
        {
            LINE_F_UNSPEC = 0,
            LINE_F_50HZ = 1,
            LINE_F_60HZ = 2
        }
        private enum MetricAccess : UInt16
        {
            AVAIL_INTERMITTEND = 0x8000,
            //The intermitted availability bit is set, if the observed values not always
            //available (e.g. only if a measurement is explicitly started)
            UPD_PERIODIC = 0x4000,
            //observed value is updated periodically
            UPD_EPISODIC = 0x2000,
            //observed value is updated episodically (exactly one update mode (UPD_) must
            MSMT_NONCONTINUOUS = 0x1000
            //indicates that the measurement is non continuous (this is
            //different from the update mode)
        }
        private enum MetricModality : Int16
        {
            METRIC_MODALITY_MANUAL = 0x4000,
            METRIC_MODALITY_APERIODIC = 0x2000,
            METRIC_MODALITY_VERIFIED = 0x1000
        }
        public enum SaFlags : UInt16
        {
            //non-CLS, but required
            SMOOTH_CURVE = 0x8000,
            //SMOOTH_CURVE, DELAYED_CURVE : used for wave presentation
            DELAYED_CURVE = 0x4000,
            STATIC_SCALE = 0x2000,
            //Scale and range specification does not change
            SA_EXT_VAL_RANGE = 0x1000
            //The non-significant bits in the sample value must be masked
        }
        public enum SaFixedValid : Int16
        {
            SA_FIX_UNSPEC = 0,
            //Not specified
            SA_FIX_INVALID_MASK = 1,
            //Invalid sample mask
            SA_FIX_PACER_MASK = 2,
            //Pace pulse detected
            SA_FIX_DEFIB_MARKER_MASK = 3,
            //Defib marker in this sample
            SA_FIX_SATURATION = 4,
            //Indicates saturation condition in this sample
            SA_FIX_QRS_MASK = 5
            //Indicates QRS trigger around this sample
        }
        public enum MeasureMode
        {
            CO2_SIDESTREAM = 0x400,
            //CO2 sidestream
            ECG_PACED = 0x200,
            //ECG_PACED: Paced mode setting
            ECG_NONPACED = 0x100,
            //ECG_NONPACED: Paced mode setting
            ECG_DIAG = 0x80,
            ECG_MONITOR = 0x40,
            //ECG_MONITOR, ECG_FILTER: ECG filter setting
            ECG_FILTER = 0x20,
            ECG_MODE_EASI = 0x8,
            //EASI derived lead
            ECG_LEAD_PRIMARY = 0x4
            //ECG primary lead
        }
        public enum UnicodeStringSpecialCharacters : UInt16
        {
            SUBSCRIPT_CAPITAL_E_CHAR = 0xe145,
            // SUBSCRIPT CAPITAL E 
            SUBSCRIPT_CAPITAL_L_CHAR = 0xe14c,
            // SUBSCRIPT CAPITAL L 
            LITER_PER_CHAR = 0xe400,
            // LITER PER - used in 4 char unit "l/min" 
            HYDROGEN_CHAR = 0xe401,
            // HYDROGEN - Used in 4 char unit "cmH2O" 
            ALARM_STAR_CHAR = 0xe40d,
            // ALARM STAR "*" 
            CAPITAL_V_WITH_DOT_ABOVE_CHAR = 0xe425,
            // CAPITAL_V_WITH_DOT_ABOVE (V with dot) 
            ZERO_WIDTH_NO_BREAK_SPACE_CHAR = 0xfeff
            // The character  = &HFFEF is used as FILL character.
            //For each wide asian character, a FILL character is
            //appended for size calculations.
        }
        private enum SimpleColor : Int16
        {
            COL_BLACK = 0,
            COL_RED = 1,
            COL_GREEN = 2,
            COL_YELLOW = 3,
            COL_BLUE = 4,
            COL_MAGENTA = 5,
            COL_CYAN = 6,
            COL_WHITE = 7,
            COL_PINK = 20,
            COL_ORANGE = 35,
            COL_LIGHT_GREEN = 50,
            COL_LIGHT_RED = 65
        }
        #endregion
    }


    

    public class DataConstants
    {
        private const byte DataExportID = 0x11;
        public const byte BOFCHAR = 0xC0;
        public const byte EOFCHAR = 0xC1;
        public const byte ESCAPECHAR = 0x7D;
        //public const byte BIT5 = 0x7C;
        public const byte BIT5COMPL = 0x20;
        private byte[] FrameAbort = { 0x7d, 0xc1};

        public static byte[] aarq_msg = {
        0x0D, 0xEC, 0x05, 0x08, 0x13, 0x01, 0x00, 0x16, 0x01, 0x02, 0x80, 0x00, 0x14, 0x02, 0x00, 0x02,
        0xC1, 0xDC, 0x31, 0x80, 0xA0, 0x80, 0x80, 0x01, 0x01, 0x00, 0x00, 0xA2, 0x80, 0xA0, 0x03, 0x00,
        0x00, 0x01, 0xA4, 0x80, 0x30, 0x80, 0x02, 0x01, 0x01, 0x06, 0x04, 0x52, 0x01, 0x00, 0x01, 0x30,
        0x80, 0x06, 0x02, 0x51, 0x01, 0x00, 0x00, 0x00, 0x00, 0x30, 0x80, 0x02, 0x01, 0x02, 0x06, 0x0C,
        0x2A, 0x86, 0x48, 0xCE, 0x14, 0x02, 0x01, 0x00, 0x00, 0x00, 0x01, 0x01, 0x30, 0x80, 0x06, 0x0C,
        0x2A, 0x86, 0x48, 0xCE, 0x14, 0x02, 0x01, 0x00, 0x00, 0x00, 0x02, 0x01, 0x00, 0x00, 0x00, 0x00,
        0x00, 0x00, 0x61, 0x80, 0x30, 0x80, 0x02, 0x01, 0x01, 0xA0, 0x80, 0x60, 0x80, 0xA1, 0x80, 0x06,
        0x0C, 0x2A, 0x86, 0x48, 0xCE, 0x14, 0x02, 0x01, 0x00, 0x00, 0x00, 0x03, 0x01, 0x00, 0x00, 0xBE,
        0x80, 0x28, 0x80, 0x06, 0x0C, 0x2A, 0x86, 0x48, 0xCE, 0x14, 0x02, 0x01, 0x00, 0x00, 0x00, 0x01,
        0x01, 0x02, 0x01, 0x02, 0x81, 0x48, 0x80, 0x00, 0x00, 0x00, 0x40, 0x00, 0x00, 0x00, 0x00, 0x00,
        0x00, 0x00, 0x80, 0x00, 0x00, 0x00, 0x20, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01,
        0x00, 0x2C, 0x00, 0x01, 0x00, 0x28, 0x80, 0x00, 0x00, 0x00, 0x00, 0x00, 0x09, 0xC4, 0x00, 0x00,
        0x09, 0xC4, 0x00, 0x00, 0x03, 0xE8, 0xFF, 0xFF, 0xFF, 0xFF, 0x60, 0x00, 0x00, 0x00, 0x00, 0x01,
        0x00, 0x0C, 0xF0, 0x01, 0x00, 0x08, 0x8C, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };

        public static byte[] aarq_msg_ext_poll = {
        0x0D, 0xEC, 0x05, 0x08, 0x13, 0x01, 0x00, 0x16, 0x01, 0x02, 0x80, 0x00, 0x14, 0x02, 0x00, 0x02,
        0xC1, 0xDC, 0x31, 0x80, 0xA0, 0x80, 0x80, 0x01, 0x01, 0x00, 0x00, 0xA2, 0x80, 0xA0, 0x03, 0x00,
        0x00, 0x01, 0xA4, 0x80, 0x30, 0x80, 0x02, 0x01, 0x01, 0x06, 0x04, 0x52, 0x01, 0x00, 0x01, 0x30,
        0x80, 0x06, 0x02, 0x51, 0x01, 0x00, 0x00, 0x00, 0x00, 0x30, 0x80, 0x02, 0x01, 0x02, 0x06, 0x0C,
        0x2A, 0x86, 0x48, 0xCE, 0x14, 0x02, 0x01, 0x00, 0x00, 0x00, 0x01, 0x01, 0x30, 0x80, 0x06, 0x0C,
        0x2A, 0x86, 0x48, 0xCE, 0x14, 0x02, 0x01, 0x00, 0x00, 0x00, 0x02, 0x01, 0x00, 0x00, 0x00, 0x00,
        0x00, 0x00, 0x61, 0x80, 0x30, 0x80, 0x02, 0x01, 0x01, 0xA0, 0x80, 0x60, 0x80, 0xA1, 0x80, 0x06,
        0x0C, 0x2A, 0x86, 0x48, 0xCE, 0x14, 0x02, 0x01, 0x00, 0x00, 0x00, 0x03, 0x01, 0x00, 0x00, 0xBE,
        0x80, 0x28, 0x80, 0x06, 0x0C, 0x2A, 0x86, 0x48, 0xCE, 0x14, 0x02, 0x01, 0x00, 0x00, 0x00, 0x01,
        0x01, 0x02, 0x01, 0x02, 0x81, 0x48, 0x80, 0x00, 0x00, 0x00, 0x40, 0x00, 0x00, 0x00, 0x00, 0x00,
        0x00, 0x00, 0x80, 0x00, 0x00, 0x00, 0x20, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01,
        0x00, 0x2C, 0x00, 0x01, 0x00, 0x28, 0x80, 0x00, 0x00, 0x00, 0x00, 0x00, 0x09, 0xC4, 0x00, 0x00,
        0x09, 0xC4, 0x00, 0x00, 0x03, 0xE8, 0xFF, 0xFF, 0xFF, 0xFF, 0x60, 0x00, 0x00, 0x00, 0x00, 0x01,
        0x00, 0x0C, 0xF0, 0x01, 0x00, 0x08, 0x8C, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00  };

        public static byte[] aarq_msg_ext_poll2 = {
        0x0D, 0xEC, 0x05, 0x08, 0x13, 0x01, 0x00, 0x16, 0x01, 0x02, 0x80, 0x00, 0x14, 0x02, 0x00, 0x02,
        0xC1, 0xDC, 0x31, 0x80, 0xA0, 0x80, 0x80, 0x01, 0x01, 0x00, 0x00, 0xA2, 0x80, 0xA0, 0x03, 0x00,
        0x00, 0x01, 0xA4, 0x80, 0x30, 0x80, 0x02, 0x01, 0x01, 0x06, 0x04, 0x52, 0x01, 0x00, 0x01, 0x30,
        0x80, 0x06, 0x02, 0x51, 0x01, 0x00, 0x00, 0x00, 0x00, 0x30, 0x80, 0x02, 0x01, 0x02, 0x06, 0x0C,
        0x2A, 0x86, 0x48, 0xCE, 0x14, 0x02, 0x01, 0x00, 0x00, 0x00, 0x01, 0x01, 0x30, 0x80, 0x06, 0x0C,
        0x2A, 0x86, 0x48, 0xCE, 0x14, 0x02, 0x01, 0x00, 0x00, 0x00, 0x02, 0x01, 0x00, 0x00, 0x00, 0x00,
        0x00, 0x00, 0x61, 0x80, 0x30, 0x80, 0x02, 0x01, 0x01, 0xA0, 0x80, 0x60, 0x80, 0xA1, 0x80, 0x06,
        0x0C, 0x2A, 0x86, 0x48, 0xCE, 0x14, 0x02, 0x01, 0x00, 0x00, 0x00, 0x03, 0x01, 0x00, 0x00, 0xBE,
        0x80, 0x28, 0x80, 0x06, 0x0C, 0x2A, 0x86, 0x48, 0xCE, 0x14, 0x02, 0x01, 0x00, 0x00, 0x00, 0x01,
        0x01, 0x02, 0x01, 0x02, 0x81, 0x48, 0x80, 0x00, 0x00, 0x00, 0x40, 0x00, 0x00, 0x00, 0x00, 0x00,
        0x00, 0x00, 0x80, 0x00, 0x00, 0x00, 0x20, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01,
        0x00, 0x2C, 0x00, 0x01, 0x00, 0x28, 0x80, 0x00, 0x00, 0x00, 0x00, 0x00, 0x03, 0xE8, 0x00, 0x00,
        0x09, 0xC4, 0x00, 0x00, 0x03, 0xE8, 0xFF, 0xFF, 0xFF, 0xFF, 0x60, 0x00, 0x00, 0x00, 0x00, 0x01,
        0x00, 0x0C, 0xF0, 0x01, 0x00, 0x08, 0x8C, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00  };

		public static byte[] aarq_msg_wave_ext_poll = {
		0x0D, 0xEC, 0x05, 0x08, 0x13, 0x01, 0x00, 0x16, 0x01, 0x02, 0x80, 0x00, 0x14, 0x02, 0x00, 0x02,
		0xC1, 0xDC, 0x31, 0x80, 0xA0, 0x80, 0x80, 0x01, 0x01, 0x00, 0x00, 0xA2, 0x80, 0xA0, 0x03, 0x00,
		0x00, 0x01, 0xA4, 0x80, 0x30, 0x80, 0x02, 0x01, 0x01, 0x06, 0x04, 0x52, 0x01, 0x00, 0x01, 0x30,
		0x80, 0x06, 0x02, 0x51, 0x01, 0x00, 0x00, 0x00, 0x00, 0x30, 0x80, 0x02, 0x01, 0x02, 0x06, 0x0C,
		0x2A, 0x86, 0x48, 0xCE, 0x14, 0x02, 0x01, 0x00, 0x00, 0x00, 0x01, 0x01, 0x30, 0x80, 0x06, 0x0C,
		0x2A, 0x86, 0x48, 0xCE, 0x14, 0x02, 0x01, 0x00, 0x00, 0x00, 0x02, 0x01, 0x00, 0x00, 0x00, 0x00,
		0x00, 0x00, 0x61, 0x80, 0x30, 0x80, 0x02, 0x01, 0x01, 0xA0, 0x80, 0x60, 0x80, 0xA1, 0x80, 0x06,
		0x0C, 0x2A, 0x86, 0x48, 0xCE, 0x14, 0x02, 0x01, 0x00, 0x00, 0x00, 0x03, 0x01, 0x00, 0x00, 0xBE,
		0x80, 0x28, 0x80, 0x06, 0x0C, 0x2A, 0x86, 0x48, 0xCE, 0x14, 0x02, 0x01, 0x00, 0x00, 0x00, 0x01,
		0x01, 0x02, 0x01, 0x02, 0x81, 0x48, 0x80, 0x00, 0x00, 0x00, 0x40, 0x00, 0x00, 0x00, 0x00, 0x00,
		0x00, 0x00, 0x80, 0x00, 0x00, 0x00, 0x20, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01,
		0x00, 0x2C, 0x00, 0x01, 0x00, 0x28, 0x80, 0x00, 0x00, 0x00, 0x00, 0x00, 0x03, 0xE8, 0x00, 0x00,
		0x09, 0xC4, 0x00, 0x00, 0x03, 0xE8, 0xFF, 0xFF, 0xFF, 0xFF, 0x60, 0x00, 0x00, 0x00, 0x00, 0x01,
		0x00, 0x0C, 0xF0, 0x01, 0x00, 0x08, 0x88, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
		0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00  };

        public static byte[] aarq_msg_wave_ext_poll2 =
        {
        0x0D, 0xFF, 0x01, 0x28, 0x05, 0x08, 0x13, 0x01, 0x00, 0x16, 0x01, 0x02, 0x80, 0x00, 0x14, 0x02,
        0x00, 0x02, 0xC1, 0xFF, 0x01, 0x16, 0x31, 0x80, 0xA0, 0x80, 0x80, 0x01, 0x01, 0x00, 0x00, 0xA2,
        0x80, 0xA0, 0x03, 0x00, 0x00, 0x01, 0xA4, 0x80, 0x30, 0x80, 0x02, 0x01, 0x01, 0x06, 0x04, 0x52,
        0x01, 0x00, 0x01, 0x30, 0x80, 0x06, 0x02, 0x51, 0x01, 0x00, 0x00, 0x00, 0x00, 0x30, 0x80, 0x02,
        0x01, 0x02, 0x06, 0x0C, 0x2A, 0x86, 0x48, 0xCE, 0x14, 0x02, 0x01, 0x00, 0x00, 0x00, 0x01, 0x01,
        0x30, 0x80, 0x06, 0x0C, 0x2A, 0x86, 0x48, 0xCE, 0x14, 0x02, 0x01, 0x00, 0x00, 0x00, 0x02, 0x01,
        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x61, 0x80, 0x30, 0x80, 0x02, 0x01, 0x01, 0xA0, 0x80, 0x60,
        0x80, 0xA1, 0x80, 0x06, 0x0C, 0x2A, 0x86, 0x48, 0xCE, 0x14, 0x02, 0x01, 0x00, 0x00, 0x00, 0x03,
        0x01, 0x00, 0x00, 0xBE, 0x80, 0x28, 0x80, 0x06, 0x0C, 0x2A, 0x86, 0x48, 0xCE, 0x14, 0x02, 0x01,
        0x00, 0x00, 0x00, 0x01, 0x01, 0x02, 0x01, 0x02, 0x81, 0x82, 0x00, 0x80, 0x80, 0x00, 0x00, 0x00,
        0x40, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x80, 0x00, 0x00, 0x00, 0x20, 0x00, 0x00, 0x00,
        0x00, 0x00, 0x00, 0x00, 0x00, 0x02, 0x00, 0x64, 0x00, 0x01, 0x00, 0x28, 0x80, 0x00, 0x00, 0x00,
        0x00, 0x00, 0x0F, 0xA0, 0x00, 0x00, 0x05, 0xB0, 0x00, 0x00, 0x05, 0xB0, 0xFF, 0xFF, 0xFF, 0xFF,
        0x60, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00, 0x0C, 0xF0, 0x01, 0x00, 0x08, 0x8E, 0x00, 0x00, 0x00,
        0x00, 0x00, 0x00, 0x00, 0x01, 0x02, 0x00, 0x34, 0x00, 0x06, 0x00, 0x30, 0x00, 0x01, 0x00, 0x21,
        0x00, 0x00, 0x00, 0x01, 0x00, 0x01, 0x00, 0x06, 0x00, 0x00, 0x00, 0xC9, 0x00, 0x01, 0x00, 0x09,
        0x00, 0x00, 0x00, 0x3C, 0x00, 0x01, 0x00, 0x05, 0x00, 0x00, 0x00, 0x10, 0x00, 0x01, 0x00, 0x2A,
        0x00, 0x00, 0x00, 0x01, 0x00, 0x01, 0x00, 0x36, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00,
        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00

        };

        public static byte[] mds_create_resp_msg = {
        0xE1, 0x00, 0x00, 0x02, 0x00, 0x02, 0x00, 0x14, 0x00, 0x01, 0x00, 0x01, 0x00, 0x0E, 0x00, 0x21,
        0x00, 0x00, 0x00, 0x00, 0x00, 0x0A, 0xBD, 0x00, 0x0D, 0x06, 0x00, 0x00 };

        public static byte[] poll_mds_request_msg = {
        0xE1, 0x00, 0x00, 0x02, 0x00, 0x01, 0x00, 0x1C, 0x00, 0x00, 0x00, 0x07, 0x00, 0x16, 0x00, 0x21,
        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x0C, 0x16, 0x00, 0x08, 0x00, 0x01, 0x00, 0x01,
        0x00, 0x21, 0x00, 0x00 };

        public static byte[] poll_request_msg = {
        0xE1, 0x00, 0x00, 0x02, 0x00, 0x01, 0x00, 0x1C, 0x00, 0x00, 0x00, 0x07, 0x00, 0x16, 0x00, 0x21,
        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x0C, 0x16, 0x00, 0x08, 0x00, 0x01, 0x00, 0x01,
        0x00, 0x06, 0x00, 0x00 };

        public static byte[] poll_request_msg2 = {
        0xE1, 0x00, 0x00, 0x02, 0x00, 0x01, 0x00, 0x1C, 0x00, 0x01, 0x00, 0x07, 0x00, 0x16, 0x00, 0x21,
        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x0C, 0x16, 0x00, 0x08, 0x00, 0x01, 0x00, 0x01,
        0x00, 0x21, 0x08, 0x0C};

        public static byte[] poll_request_msg3 =
        {
            0xe1, 0x00, 0x00, 0x02, 0x00, 0x01, 0x00, 0x1c, 0x00, 0x06, 0x00, 0x07, 0x00, 0x16, 0x00, 0x21,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x0c, 0x16, 0x00, 0x08, 0x00, 0x05, 0x00, 0x01,
            0x00, 0x2a, 0x08, 0x07
        };

        public static byte[] poll_request_msg4 =
        {
            0xe1, 0x00, 0x00, 0x02, 0x00, 0x01, 0x00, 0x1c, 0x00, 0x10, 0x00, 0x07, 0x00, 0x16, 0x00, 0x21,
            0x00, 0x10, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x0c, 0x16, 0x00, 0x08, 0x00, 0x0f,
            0x00, 0x01, 0x00, 0x20, 0x00, 0x36, 0x08, 0x11
        };

        public static byte[] ext_poll_request_msg3 =
        {
            0xe1, 0x00, 0x00, 0x02, 0x00, 0x01, 0x00, 0x28, 0x00, 0x04, 0x00, 0x07, 0x00, 0x22, 0x00, 0x21,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0xf1, 0x3b, 0x00, 0x14, 0x00, 0x03, 0x00, 0x01,
            0x00, 0x09, 0x00, 0x00, 0x00, 0x01, 0x00, 0x08, 0xf1, 0x3e, 0x00, 0x04, 0x00, 0x49, 0x3e, 0x00
        };

        public static byte[] ext_poll_request_msg = {
        0xE1, 0x00, 0x00, 0x02, 0x00, 0x01, 0x00, 0x20, 0x00, 0x01, 0x00, 0x07, 0x00, 0x1A, 0x00, 0x21,
        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0xF1, 0x3B, 0x00, 0x0C, 0x00, 0x01, 0x00, 0x01,
        0x00, 0x06, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };

        public static byte[] ext_poll_request_msg2 = {
        0xE1, 0x00, 0x00, 0x02, 0x00, 0x01, 0x00, 0x20, 0x00, 0x01, 0x00, 0x07, 0x00, 0x1A, 0x00, 0x21,
        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0xF1, 0x3B, 0x00, 0x0E, 0x00, 0x01, 0x00, 0x01,
        0x00, 0x06, 0x00, 0x01, 0x00, 0x04, 0x80, 0x00, 0x00, 0x00};

        public static byte[] ext_poll_request_msg4 =
        {
            0xe1, 0x00, 0x00, 0x02, 0x00, 0x01, 0x00, 0x28, 0x00, 0x03, 0x00, 0x07, 0x00, 0x22, 0x00, 0x21,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0xf1, 0x3b, 0x00, 0x14, 0x00, 0x02, 0x00, 0x01,
            0x00, 0x06, 0x00, 0x00, 0x00, 0x01, 0x00, 0x08, 0xf1, 0x3e, 0x00, 0x04, 0x00, 0x49, 0x3e, 0x00
        };

        public static byte[] ext_poll_request_msg5 =
        {
            0xE1, 0x00, 0x00, 0x02, 0x00, 0x01, 0x00, 0x28, 0x00, 0x01, 0x00, 0x07, 0x00, 0x22, 0x00, 0x21,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0xF1, 0x3B, 0x00, 0x14, 0x00, 0x01, 0x00, 0x01,
            0x00, 0x06, 0x00, 0x00, 0x00, 0x01, 0x00, 0x08, 0xF1, 0x3E, 0x00, 0x04, 0x00, 0x03, 0xA9, 0x80
        };

        public static byte[] ext_poll_request_wave_msg3 =
        {
            0xE1, 0x00, 0x00, 0x02, 0x00, 0x01, 0x00, 0x28, 0x00, 0x01, 0x00, 0x07, 0x00, 0x22, 0x00, 0x21,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0xF1, 0x3B, 0x00, 0x14, 0x00, 0x02, 0x00, 0x01,
            0x00, 0x09, 0x00, 0x00, 0x00, 0x01, 0x00, 0x08, 0xF1, 0x3E, 0x00, 0x04, 0x00, 0x03, 0xA9, 0x80
        };

        public static byte[] ext_poll_request_wave_msg =
		{
			0xe1, 0x00, 0x00, 0x02, 0x00, 0x01, 0x00, 0x28, 0x00, 0x04, 0x00, 0x07, 0x00, 0x22, 0x00, 0x21,
			0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0xf1, 0x3b, 0x00, 0x14, 0x00, 0x03, 0x00, 0x01,
			0x00, 0x09, 0x00, 0x00, 0x00, 0x01, 0x00, 0x08, 0xf1, 0x3e, 0x00, 0x04, 0x00, 0x49, 0x3e, 0x00
		};

        public static byte[] ext_poll_request_wave_msg2 =
        {
            0xe1, 0x00, 0x00, 0x02, 0x00, 0x01, 0x00, 0x28, 0x00, 0x04, 0x00, 0x07, 0x00, 0x22, 0x00, 0x21,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0xf1, 0x3b, 0x00, 0x14, 0x00, 0x03, 0x00, 0x01,
            0x00, 0x09, 0x00, 0x00, 0x00, 0x01, 0x00, 0x08, 0xf1, 0x3e, 0x00, 0x04, 0x00, 0x49, 0x3e, 0x00
        };
        
        public static byte[] ext_poll_request_alert_msg =
        {
            0xe1, 0x00, 0x00, 0x02, 0x00, 0x01, 0x00, 0x28, 0x00, 0x05, 0x00, 0x07, 0x00, 0x22, 0x00, 0x21,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0xf1, 0x3b, 0x00, 0x14, 0x00, 0x04, 0x00, 0x01,
            0x00, 0x36, 0x08, 0x01, 0x00, 0x01, 0x00, 0x08, 0xf1, 0x3e, 0x00, 0x04, 0x00, 0x49, 0x3e, 0x00
        };

        public static byte[] get_rtsa_prio_msg = {
        0xE1, 0x00, 0x00, 0x02, 0x00, 0x01, 0x00, 0x16, 0x00, 0x00, 0x00, 0x03, 0x00, 0x10, 0x00, 0x21,
        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00, 0x02, 0xF2, 0x3A };

        public static byte[] keep_alive_msg =
        {
           0xe1, 0x00, 0x00, 0x02, 0x00, 0x01, 0x00, 0x1c, 0x00, 0x01, 0x00, 0x07, 0x00, 0x16, 0x00, 0x21,
           0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x0c, 0x16, 0x00, 0x08, 0x00, 0x01, 0x00, 0x01,
           0x00, 0x21, 0x08, 0x0c
        };

        public static byte[] set_rtsa_prio_msg =
        {
            0xE1, 0x00, 0x00, 0x02, 0x00, 0x01, 0x00, 0x22, 0x00, 0x00, 0x00, 0x05, 0x00, 0x1C, 0x00, 0x21,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00, 0x0E, 0x00, 0x00, 0xF2, 0x3A,
            0x00, 0x08, 0x00, 0x01, 0x00, 0x04, 0x00, 0x02, 0x01, 0x02
        };
        
        public static byte[] keep_alive_msg2 =
        {
            0xe1, 0x00, 0x00, 0x02, 0x00, 0x01, 0x00, 0x1c, 0x00, 0x08, 0x00, 0x07, 0x00, 0x16, 0x00, 0x21,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x0c, 0x16, 0x00, 0x08, 0x00, 0x07, 0x00, 0x01,
            0x00, 0x36, 0x08, 0x11
        };

        public static byte[] rlrq_msg = {
        0x09, 0x18, 0xC1, 0x16, 0x61, 0x80, 0x30, 0x80, 0x02, 0x01, 0x01, 0xA0, 0x80, 0x62, 0x80, 0x80,
        0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };

        public static byte[] rlrq_resp_msg = {
        0x0A, 0x18, 0xC1, 0x16, 0x61, 0x80, 0x30, 0x80, 0x02, 0x01, 0x01, 0xA0, 0x80, 0x63, 0x80, 0x80,
        0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };

        public static byte[] assoc_abort_resp_msg = {
        0x19, 0x2E, 0x11, 0x01, 0x03, 0xC1, 0x29, 0xA0, 0x80, 0xA0, 0x80, 0x30, 0x80, 0x02, 0x01, 0x01,
        0x06, 0x02, 0x51, 0x01, 0x00, 0x00, 0x00, 0x00, 0x61, 0x80, 0x30, 0x80, 0x02, 0x01, 0x01, 0xA0,
        0x80, 0x64, 0x80, 0x80, 0x01, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };

        //SessionHeader
        public const byte CN_SPDU_SI = 0x0D;
        public const byte AC_SPDU_SI = 0x0E;
        public const byte RF_SPDU_SI = 0x0C;
        public const byte FN_SPDU_SI = 0x09;
        public const byte DN_SPDU_SI = 0x0A;
        public const byte AB_SPDU_SI = 0x19;
        public const byte DA_SPDU_SI = 0xE1;

        //ROapdus
        public const byte ROIV_APDU  = 1;
        public const byte RORS_APDU  = 2;
        public const byte ROER_APDU  = 3;
        public const byte RORLS_APDU = 5;

        //RORSapdu
        public const byte CMD_EVENT_REPORT = 0;
        public const byte CMD_CONFIRMED_EVENT_REPORT = 1;
        public const byte CMD_GET = 3;
        public const byte CMD_SET = 4;
        public const byte CMD_CONFIRMED_SET = 5;
        public const byte CMD_CONFIRMED_ACTION = 7;

        //RORLSapdu
        public const byte RORLS_FIRST = 1; /* set in the first message */
        public const byte RORLS_NOT_FIRST_NOT_LAST = 2;
        public const byte RORLS_LAST = 3; /* last RORLSapdu, one RORSapdu to follow */

        //ActionType
        public const ushort NOM_ACT_POLL_MDIB_DATA = 3094; //Single poll
        public const ushort NOM_ACT_POLL_MDIB_DATA_EXT = 61755; //Extended poll
            

        //ReceiveState
        public const byte STATE_SEARCH_FRAME = 1;
        public const byte STATE_READ_FRAME = 2;
        public const byte STATE_FINISHED_FRAME = 3;

        //MeasurementState
        public const int INVALID = 0x8000;
        public const int QUESTIONABLE = 0x4000;
        public const int UNAVAILABLE = 0x2000;
        public const int CALIBRATION_ONGOING = 0x1000;
        public const int TEST_DATA = 0x0800;
        public const int DEMO_DATA = 0x0400;
        public const int VALIDATED_DATA = 0x0080;
        public const int EARLY_INDICATION = 0x0040;
        public const int MSMT_ONGOING = 0x0020;
        public const int MSMT_STATE_IN_ALARM = 0x0002;
        public const int MSMT_STATE_AL_INHIBITED = 0x0001;

        public const int FLOATTYPE_NAN = 0x007FFFFF;
        public const int FLOATTYPE_NRes = 0x800000;
        public const int FLOATTYPE_POSITIVE_INFINITY = 0x7FFFFE;
        public const int FLOATTYPE_NEGATIVE_INFINITY = 0x800002;


        //MetricCategory
        public const byte MCAT_UNSPEC = 0;
        public const byte AUTO_MEASUREMENT = 1;
        public const byte MANUAL_MEASUREMENT = 2;
        public const byte AUTO_SETTING = 3;
        public const byte MANUAL_SETTING = 4;
        public const byte AUTO_CALCULATION = 5;
        public const byte MANUAL_CALCULATION = 6;
        public const byte MULTI_DYNAMIC_CAPABILITIES = 50;
        public const byte AUTO_ADJUST_PAT_TEMP = 128;
        public const byte MANUAL_ADJUST_PAT_TEMP = 129;
        public const byte AUTO_ALARM_LIMIT_SETTING = 130;

        public const int AVAIL_INTERMITTEND = 0x8000;
        public const int UPD_PERIODIC = 0x4000;
        public const int UPD_EPISODIC = 0x2000;
        public const int MSMT_NONCONTINUOUS = 0x1000;

        //PollProfileExtOptions
        public const uint POLL_EXT_PERIOD_NU_1SEC = 0x80000000;
        public const int POLL_EXT_PERIOD_NU_AVG_12SEC = 0x40000000;
        public const int POLL_EXT_PERIOD_NU_AVG_60SEC = 0x20000000;
        public const int POLL_EXT_PERIOD_NU_AVG_300SEC = 0x10000000;
        public const int POLL_EXT_PERIOD_RTSA = 0x08000000;
        public const int POLL_EXT_ENUM = 0x04000000;
        public const int POLL_EXT_NU_PRIO_LIST = 0x02000000;
        public const int POLL_EXT_DYN_MODALITIES = 0x01000000;

        //-----------------------------------------------------------------------------

        public const ushort NOM_MOC_VMS_MDS = 33;
        //MDS
        public const int NOM_ATTR_METRIC_SPECN = 2367;
        public const int NOM_ATTR_ID_HANDLE = 2337;
        public const int NOM_ATTR_ID_LABEL = 2340;
        public const int NOM_ATTR_ID_LABEL_STRING = 2343;
        public const int NOM_ATTR_NU_CMPD_VAL_OBS = 2379;
        public const int NOM_ATTR_NU_VAL_OBS = 2384;
        public const int NOM_ATTR_SYS_TYPE = 0x986;
		public const int NOM_ATTR_SA_CALIB_I16 = 0x964;
		//Compound Sample Array Observed Value
		public const int NOM_ATTR_SA_CMPD_VAL_OBS = 0x967;
		//Sample Array Physiological Range
		public const int NOM_ATTR_SA_RANGE_PHYS_I16 = 0x96a;
		//Sample Array Specification
		public const int NOM_ATTR_SA_SPECN = 0x96d;
		//Sample Array Observed Value
		public const int NOM_ATTR_SA_VAL_OBS = 0x96e;
		//Scale and Range Specification
		public const int NOM_ATTR_SCALE_SPECN_I16 = 0x96f;
            
        //Date and Time
        public const int NOM_ATTR_TIME_ABS = 0x987;
        //Sample Period
        public const int NOM_ATTR_TIME_PD_SAMP = 0x98d;
        //Relative Time
        public const int NOM_ATTR_TIME_REL = 0x98f;
        //Absolute Time Stamp
        public const int NOM_ATTR_TIME_STAMP_ABS = 0x990;
        //Relative Time Stamp
        public const int NOM_ATTR_TIME_STAMP_REL = 0x991;
        //Patient Date of Birth
        public const int NOM_ATTR_PT_DOB = 0x958;
        //Patient ID
        public const int NOM_ATTR_PT_ID = 0x95a;
        //Family Name
        public const int NOM_ATTR_PT_NAME_FAMILY = 0x95c;
        //Given Name
        public const int NOM_ATTR_PT_NAME_GIVEN = 0x95d;
        //Patient Sex
        public const int NOM_ATTR_PT_SEX = 0x961;
            
            
        //-----------------------------------------------------------------------------

        public const uint MDDL_VERSION1 = 0x80000000;
        public const int NOM_ATTR_POLL_PROFILE_EXT = 61441;
        public const byte NOM_POLL_PROFILE_SUPPORT = 1;

        public const byte VAL_METRIC_SPEC = 1;
        public const byte VAL_LABEL = 2;
        public const byte VAL_LABEL_STRING = 4;
        public const byte VAL_VALUE = 8;

        public enum WavesIDLabels : UInt32
        {
            NLS_NOM_ECG_ELEC_POTL = 0x00020100,
            NLS_NOM_ECG_ELEC_POTL_I = 0x00020101,
            NLS_NOM_ECG_ELEC_POTL_II = 0x00020102,
            NLS_NOM_ECG_ELEC_POTL_III = 0x0002013D,
            NLS_NOM_ECG_ELEC_POTL_AVR = 0x0002013E,
            NLS_NOM_ECG_ELEC_POTL_AVL = 0x0002013F,
            NLS_NOM_ECG_ELEC_POTL_AVF = 0x00020140,
            NLS_NOM_ECG_ELEC_POTL_V1 = 0x00020103,
            NLS_NOM_ECG_ELEC_POTL_V2 = 0x00020104,
            NLS_NOM_ECG_ELEC_POTL_V3 = 0x00020105,
            NLS_NOM_ECG_ELEC_POTL_V4 = 0x00020106,
            NLS_NOM_ECG_ELEC_POTL_V5 = 0x00020107,
            NLS_NOM_ECG_ELEC_POTL_V6 = 0x00020108,
            NLS_NOM_PULS_OXIM_PLETH = 0x00024BB4,
            NLS_NOM_PRESS_BLD_ART = 0x00024A10,
            NLS_NOM_PRESS_BLD_ART_ABP = 0x00024A14,
            NLS_NOM_PRESS_BLD_VEN_CENT = 0x00024A44,
            NLS_NOM_RESP = 0x00025000,
            NLS_NOM_AWAY_CO2 = 0x000250AC,
            NLS_NOM_PRESS_AWAY = 0x000250F0,
            NLS_NOM_FLOW_AWAY = 0x000250D4,
            NLS_EEG_NAMES_EEG_CHAN1_LBL = 0x800F5401,
            NLS_EEG_NAMES_EEG_CHAN2_LBL = 0x800F5402,
            NLS_EEG_NAMES_EEG_CHAN3_LBL = 0x800F5432,
            NLS_EEG_NAMES_EEG_CHAN4_LBL = 0x800F5434,
            NLS_NOM_PRESS_INTRA_CRAN = 0x00025808,
            NLS_NOM_PRESS_INTRA_CRAN_2 = 0x0002F0B8,
            NLS_NOM_TEMP_BLD = 0x0002E014

        }
    }

    #region

    // This structure is big endian on the wire
    [StructLayout(LayoutKind.Sequential, Pack = 1, Size = 44, CharSet = CharSet.Ansi)]
    public class SinglePollPacketResult
    {
        public SessionHeader session_hdr = new SessionHeader();
        public ROapdus remoteop_hdr = new ROapdus();
        public RORSapdu remoteop_cmd = new RORSapdu();
        public ActionResult action_result = new ActionResult();
        public PollMdibDataReply mdib_data = new PollMdibDataReply();
        public PollInfoList pollinfo_list = new PollInfoList(); //null placeholder
         
    }

   
    [StructLayout(LayoutKind.Sequential, Pack = 1, Size = 46, CharSet = CharSet.Ansi)]
    public class SinglePollLinkedPacketResult
    {
        public SessionHeader session_hdr = new SessionHeader();
        public ROapdus remoteop_hdr = new ROapdus();
        public RORLSapdu remoteop_cmd = new RORLSapdu();
        public ActionResult action_result = new ActionResult();
        public PollMdibDataReply mdib_data = new PollMdibDataReply();
        public PollInfoList pollinfo_list = new PollInfoList(); //null placeholder

    }

    [StructLayout(LayoutKind.Sequential, Pack = 1, Size = 48, CharSet = CharSet.Ansi)]
    public class ExtPollLinkedPacketResult
    {
        public SessionHeader session_hdr = new SessionHeader();
        public ROapdus remoteop_hdr = new ROapdus();
        public RORLSapdu remoteop_cmd = new RORLSapdu();
        public ActionResult action_result = new ActionResult();
        public PollMdibDataReplyExt mdib_data = new PollMdibDataReplyExt();
        public PollInfoList pollinfo_list = new PollInfoList(); //null placeholder

    }


    [StructLayout(LayoutKind.Sequential, Pack = 1, Size = 4, CharSet = CharSet.Ansi)]
    public class RawFrameHdr
    {
        public byte protocol_id;
        public byte msg_type;
        public ushort length;
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1, Size = 4, CharSet = CharSet.Ansi)]
    public class SessionHeader
    {
        public byte type;
        public byte length;
        public ushort length2;
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1, Size = 4, CharSet = CharSet.Ansi)]
    public class ROapdus
    {
        public ushort ro_type;
        public ushort length;
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1, Size = 6, CharSet = CharSet.Ansi)]
    public class RORSapdu
    {
        public ushort invoke_id;
        public ushort command_type;
        public ushort length;
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1, Size = 6, CharSet = CharSet.Ansi)]
    public class ROIVapdu
    {
        public ushort inovke_id;
        public ushort command_type;
        public ushort length;
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1, Size = 8, CharSet = CharSet.Ansi)]
    public class RORLSapdu
    {
        public byte state;
        public byte count;
        public RORSapdu apdu;
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1, Size = 4, CharSet = CharSet.Ansi)]
    public class GlbHandle
    {
        public ushort context_id;
        public ushort handle;
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1, Size = 6, CharSet = CharSet.Ansi)]
    public class ManagedObjectId
    {
        public ushort m_obj_class;
        public GlbHandle m_obj_inst;
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1, Size = 8, CharSet = CharSet.Ansi)]
    public class AbsoluteTime
    {
        public byte century;
        public byte year;
        public byte month;
        public byte day;
        public byte hour;
        public byte minute;
        public byte second;
        public byte fraction;
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1, Size = 4, CharSet = CharSet.Ansi)]
    public class ObjectType
    {
        public ushort partition;
        public ushort code;
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1, Size = 6, CharSet = CharSet.Ansi)]
    public class Ava
    {
        public ushort attribute_id;
        public ushort length;
        public ushort attribute_val; //placeholder
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1, Size = 10, CharSet = CharSet.Ansi)]
    public class AttributeList
    {
        public ushort count;
        public ushort length;
        //public Ava [] value = new Ava[1];
        public Ava value1; //null placeholder
        public byte[] avaobjectsarray;
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1, Size = 12, CharSet = CharSet.Ansi)]
    public class ObservationPoll
    {
        public ushort obj_handle;
        public AttributeList attributes;
        public byte[] avaobjectsarray;
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1, Size = 18, CharSet = CharSet.Ansi)]
    public class SingleContextPoll
    {
        public ushort context_id;
        public ushort count;
        public ushort length;
        //public ObservationPoll [] value = new ObservationPoll[1];
        public ObservationPoll value1; //null placeholder
        public byte[] obpollobjectsarray;
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1, Size = 22, CharSet = CharSet.Ansi)]
    public class PollInfoList
    {
        public ushort count;
        public ushort length;
        //public SingleContextPoll [] value= new SingleContextPoll[1];
        public SingleContextPoll value1; //null placeholder
        public byte[] scpollarray;
    };

        
    [StructLayout(LayoutKind.Sequential, Pack = 1, Size = 20, CharSet = CharSet.Ansi)]
    public class PollMdibDataReply
    {
        public ushort poll_number;
        public uint rel_time_stamp;
        public AbsoluteTime abs_time_stamp;
        public ObjectType type = new ObjectType();
        public ushort polled_attr_grp;
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1, Size = 22, CharSet = CharSet.Ansi)]
    public class PollMdibDataReplyExt
    {
        public ushort poll_number;
        public ushort sequence_no;
        public uint rel_time_stamp;
        public AbsoluteTime abs_time_stamp;
        public ObjectType type =  new ObjectType();
        public ushort polled_attr_grp;
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1, Size = 10, CharSet = CharSet.Ansi)]
    public class ActionResult
    {
        public ManagedObjectId objectid;
        public ushort action_type;
        public ushort length;

    };

    [StructLayout(LayoutKind.Sequential, Pack = 1, Size = 8, CharSet = CharSet.Ansi)]
    public class ReceiveState
    {
        public uint state;
        public uint position;
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1, Size = 10, CharSet = CharSet.Ansi)]
    public class NuObsValue
    {
        public ushort physio_id;
        public ushort state;
        public ushort unit_code;
        public uint value;
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1, Size = 14, CharSet = CharSet.Ansi)]
    public class NuObsValueCmp
    {
        public ushort count;
        public ushort length;
        //public NuObsValue [] value = new NuObsValue[1];
        public NuObsValue value1;
    };

	[StructLayout(LayoutKind.Sequential, Pack = 1, Size = 7, CharSet = CharSet.Ansi)]
	public class SaObsValue
	{
		public ushort physio_id;
		public ushort state;
		public ushort length;
		//public byte [] value = new byte[1];  
		public byte value1;
	};

	[StructLayout(LayoutKind.Sequential, Pack = 1, Size = 11, CharSet = CharSet.Ansi)]
	public class SaObsValueCmp
	{
		public ushort count;
		public ushort length;
		//public SaObsValue [] value = new SaObsValue[1];
		public SaObsValue value1;
	};

    [StructLayout(LayoutKind.Sequential, Pack = 1, Size = 8, CharSet = CharSet.Ansi)]
    public class SaSpec
    {
        public ushort array_size;
        public byte sample_size;
        public byte significant_bits;
        public ushort SaFlags;
        public ushort obpoll_handle;
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1, Size = 24, CharSet = CharSet.Ansi)]
    public class ScaleRangeSpec16
    {
        public double lower_absolute_value;
        public double upper_absolute_value;
        public ushort lower_scaled_value;
        public ushort upper_scaled_value;
        public ushort obpoll_handle;
        public ushort physio_id;
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1, Size = 28, CharSet = CharSet.Ansi)]
    public class SaCalibData16
    {
        public double lower_absolute_value;
        public double upper_absolute_value;
        public ushort lower_scaled_value;
        public ushort upper_scaled_value;
        public ushort increment;
        public ushort cal_type;
        public ushort obpoll_handle;
        public ushort physio_id;
    };


    [StructLayout(LayoutKind.Sequential, Pack = 1, Size = 2, CharSet = CharSet.Ansi)]
    public class MetricStructure
    {
        public byte ms_struct;
        public byte ms_comp_no;
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1, Size = 12, CharSet = CharSet.Ansi)]
    public class MetricSpec
    {
       public uint update_period;
       public ushort category;
       public ushort access;
       MetricStructure structure;
       public ushort relevance;
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1, Size = 4, CharSet = CharSet.Ansi)]
    public class StringMP
    {
        public ushort length;
        //public ushort [] value = new ushort[1];
        public ushort value1;
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1, Size = 14, CharSet = CharSet.Ansi)]
    public class PollProfileExt
    {
        public uint options;
        AttributeList ext_attr;
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1, Size = 34, CharSet = CharSet.Ansi)]
    public class PollProfileSupport
    {
        public uint poll_profile_revision;
        public uint min_poll_period;
        public uint max_mtu_rx;
        public uint max_mtu_tx;
        public uint max_bw_tx;
        public uint options;
        AttributeList optional_packages;
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1, Size = 34, CharSet = CharSet.Ansi)]
    public class MdseUserInfoStd
    {
        public uint protocol_version;
        public uint nomenclature_version;
        public uint functional_units;
        public uint system_type;
        public uint startup_mode;
        public uint option_list;
        AttributeList supported_aprofiles;
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1, Size = 44, CharSet = CharSet.Ansi)]
    public class MDSCreateEvenReport
    {
        public SessionHeader session_hdr = new SessionHeader();
        public ROapdus remoteop_hdr = new ROapdus();
        public ROIVapdu remoteinvoke_hdr = new ROIVapdu();
        public EventReportArgument eventreport_hdr = new EventReportArgument();
        public ManagedObjectId objectid;
        public AttributeList attributes;

    };

    [StructLayout(LayoutKind.Sequential, Pack = 1, Size = 14, CharSet = CharSet.Ansi)]
    public class EventReportArgument
    {
        public ManagedObjectId managed_object;
        public uint relative_time;
        public ushort event_type;
        public ushort length;

    };

    //-----------------------------------------------------------------------------

    [StructLayout(LayoutKind.Sequential, Pack = 1, Size = 40, CharSet = CharSet.Ansi)]
    public class NumericObject
    {
        public uint validity;
        public ushort pollNumber;
        public uint label;
        NuObsValue value;
        MetricSpec spec;
        StringMP labelString;
        
    };

#endregion
    

}
