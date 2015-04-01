using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConAnecoop.AnecoopConsultaExpediente;
using ConAnecoop.AnecoopLogin;
using MySql.Data.MySqlClient;
using System.IO;
using System.Reflection;


namespace ConAnecoop
{
    class Program
    {
        static void Main(string[] args)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["AriAgro"].ConnectionString;
            string cooperativa = ConfigurationSettings.AppSettings["cooperativa"];
            string usuario = ConfigurationSettings.AppSettings["usuario"];
            string password = ConfigurationSettings.AppSettings["password"];

            // obtención de lina de comandos
            if (args.Count() < 3)
            {
                Console.WriteLine("Número de parámetros insuficiente");
            }

            string sFechaInicio = args[0];
            string sFechaFin = args[1];

            int dia = int.Parse(sFechaInicio.Split('/')[0]);
            int mes = int.Parse(sFechaInicio.Split('/')[1]);
            int anyo = int.Parse(sFechaInicio.Split('/')[2]);
            DateTime fechaInicio = new DateTime(anyo, mes, dia);

            dia = int.Parse(sFechaFin.Split('/')[0]);
            mes = int.Parse(sFechaFin.Split('/')[1]);
            anyo = int.Parse(sFechaFin.Split('/')[2]);
            DateTime fechaFin = new DateTime(anyo, mes, dia);

            string campanya = args[2];

            bool informa = false;

            if (args.Count() > 3) informa = args[3].Equals("v", StringComparison.OrdinalIgnoreCase);

            MySqlConnection conn = new MySqlConnection(connectionString);
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();

            LoginSoapClient lgc = new AnecoopLogin.LoginSoapClient("LoginSoap");
            if (informa) Console.WriteLine("Pidiendo Login...");
            ServiceTicket stk = lgc.CheckLogin(usuario, cooperativa, password);
            string sky = stk.SessionKey;
            if (informa) Console.WriteLine("Permiso recibido, clave de sesión ({0})", sky);
            
            JSVConsultaExpedienteSoapClient cliente = new JSVConsultaExpedienteSoapClient("JSVConsultaExpedienteSoap");
            if (informa) Console.WriteLine("Pidiendo expedientes, espere...");
            DateTime dt1 = DateTime.Now;
            DescargaPedidosResultExpediente[] res = cliente.DescargaPedidos(cooperativa, sky, fechaInicio, fechaFin,null, null, null, null, null, null,campanya);
            DateTime dt2 = DateTime.Now;
            if (informa) Console.WriteLine("Tiempo de petición --> Inicio: {0:dd/MM/yyyy HH:mm:ss} Fin:{1:dd/MM/yyyy HH:mm:ss}", dt1, dt2);
            int totReg = res.Length;
            int numReg = 0;
            foreach (DescargaPedidosResultExpediente exp in res)
            {
                string expediente_id = exp.EXPEDIENTE_ID; 
                string pdls_id = exp.pdls_id; 
                string linea_expediente = exp.LINEA_EXPEDIENTE; 
                string codigo_campanya = exp.CODIGO_CAMPANYA; 
                string periodo = exp.PERIODO; 
                decimal codigo_cooperativa_gestora = exp.CODIGO_COOPERATIVA_GESTORA; 
                string nombre_gestora = exp.NOMBRE_GESTORA; 
                decimal codigo_cooperativa_carga = exp.CODIGO_COOPERATIVA_CARGA; 
                string nombre_carga = exp.NOMBRE_CARGA; 
                string pto_carga = exp.PTO_CARGA; 
                string nombre_pto_carga = exp.NOMBRE_PTO_CARGA; 
                string numero_salida_cooperativa = exp.NUMERO_SALIDA_COOPERATIVA; 
                string nlinea_salida_cooperativa = exp.N_LINEA_SALIDA_COOPERATIVA; 
                decimal n_pedido_aneccop = exp.N_PEDIDO_ANECOOP; 
                decimal n_pedido = exp.N_PEDIDO; 
                decimal n_linea = exp.N_LINEA; 
                string tipo_expediente = exp.TIPO_EXPEDIENTE; 
                string estado_coop_expediente = exp.ESTADO_COOP_EXPEDIENTE; 
                string expediente_reenviado = exp.EXPEDIENTE_REENVIADO; 
                string codigo_delegacion = exp.CODIGO_DELEGACION; 
                string nombre_delegacion = exp.NOMBRE_DELEGACION; 
                string codigo_area = exp.CODIGO_AREA; 
                string nombre_area = exp.NOMBRE_AREA; 
                DateTime fecha_salida = exp.FECHA_SALIDA; 
                string matricula = exp.MATRICULA; 
                string tipo_vehiculo = exp.TIPO_VEHICULO; 
                string categoria = exp.CATEGORIA; 
                string nombre_categoria = exp.NOMBRE_CATEGORIA; 
                string codigo_confeccion = exp.CODIGO_CONFECCION; 
                string nombre_confeccion = exp.NOMBRE_CONFECCION; 
                string codigo_uventa = exp.CODIGO_UVENTA; 
                string nombre_uventa = exp.NOMBRE_UVENTA; 
                string codigo_etransporte = exp.CODIGO_ETRANSPORTE; 
                string nombre_etransporte = exp.NOMBRE_ETRANSPORTE; 
                string codigo_modelo = exp.CODIGO_MODELO; 
                string nombre_modelo = exp.NOMBRE_MODELO; 
                string codigo_material = exp.CODIGO_MATERIAL; 
                string nombre_material = exp.NOMBRE_MATERIAL; 
                string codigo_marca = exp.CODIGO_MARCA; 
                string nombre_marca = exp.NOMBRE_MARCA; 
                string codigo_variedad = exp.CODIGO_VARIEDAD; 
                string nombre_variedad = exp.NOMBRE_VARIEDAD; 
                string codigo_articulo = exp.CODIGO_ARTICULO; 
                string nombre_articulo = exp.NOMBRE_ARTICULO; 
                string ean13 = exp.EAN13; 
                string codigo_tipo_articulo = exp.CODIGO_TIPO_ARTICULO;
                string codigo_cliente = exp.CODIGO_CLIENTE;
                string nombre_cliente = exp.NOMBRECLIENTE;
                string codigo_pais = exp.CODIGO_PAIS;
                string codigo_divisa_cliente = exp.CODIGO_DIVISA_CLIENTE;
                string codigo_producto = exp.CODIGO_PRODUCTO;
                string nombre_producto = exp.NOMBRE_PRODUCTO;
                string codigo_calibre = exp.CODIGO_CALIBRE;
                string nombre_calibre = exp.NOMBRE_CALIBRE;
                decimal npalet = exp.NPALET;
                string tipo_palet = exp.TIPO_PALET;
                string nombre_tipo_palet = exp.NOMBRE_TIPO_PALET;
                decimal ncajas = exp.NCAJAS;
                decimal peso_neto = exp.PESO_NETO;
                string unidad_peso = exp.UNIDAD_PESO;
                decimal precio_comercial = exp.PRECIO_COMERCIAL;
                string codigo_divisa = exp.CODIGO_DIVISA;
                string liquidacion_son = exp.LIQUIDADO_SON;
                string estado_coop_liquidacion = exp.ESTADO_COOP_LIQUIDACION;
                string liquidacion_agrupada_son = exp.LIQUIDACION_AGRUPADA_SON;
                string codigo_grupo_liquidacion = exp.CODIGO_GRUPO_LIQUIDACION;
                decimal num_liq = exp.NUM_LIQ;
                string fra_liq = exp.FRA_LIQ;
                string fra_liq_complementaria = exp.FRA_LIQ_COMPLEMENTA;
                string carta_liq = exp.CARTA_LIQ;
                string registro_liq = exp.REGISTRO_LIQ;
                decimal importe_liq = exp.IMPORTE_LIQ;
                decimal importe_iva_liq = exp.IMPORTE_IVA_LIQ;
                string tipo_iva_liq = exp.TIPO_IVA_LIQ;
                DateTime fecha_liq = exp.FECHA_LIQ;
                DateTime fecha_cambio_liq = exp.FECHA_CAMBIO_LIQ;
                decimal cambio_liq = exp.CAMBIO_LIQ;
                DateTime fecha_sc_liq = exp.FECHA_SC_LIQ;
                decimal porcentaje_iva_liq = exp.PORCENT_IVA_LIQ;
                decimal importe2_iva_liq = exp.IMPORTE2_IVA_LIQ;
                string cobrado_son = exp.COBRADO_SON;
                string pagado_son = exp.PAGADO_SON;
                decimal valor_mercancia = exp.VALOR_MERCANCIA;
                decimal valor_confeccion = exp.VALOR_CONFECCION;
                decimal numero_salida_aneccop = exp.NUMERO_SALIDA_ANECOOP;
                string salida_linea_id = exp.SALIDALINEAID;

                // Grabación en base de datos
                // VRS 1.0.0.2
                // Se ha eliminado del update "numero_salida_cooperativa='{11}', "
                string sql = @"insert into anecoop(expediente_id, pdls_id, linea_expediente, codigo_campanya, 
                    periodo, codigo_cooperativa_gestora, nombre_gestora, codigo_cooperativa_carga, 
                    nombre_carga, pto_carga, nombre_pto_carga, numero_salida_cooperativa, 
                    nlinea_salida_cooperativa, n_pedido_aneccop, n_pedido, n_linea,
                    tipo_expediente, estado_coop_expediente, expediente_reenviado, codigo_delegacion,
                    nombre_delegacion, codigo_area, nombre_area, fecha_salida, matricula, tipo_vehiculo,
                    categoria, nombre_categoria, codigo_confeccion, nombre_confeccion, 
                    codigo_uventa, nombre_uventa, codigo_etrasnsporte, nombre_etransporte, codigo_modelo,
                    nombre_modelo, codigo_material, nombre_material, codigo_marca, nombre_marca,
                    codigo_variedad, nombre_variedad, codigo_articulo, nombre_articulo, ean13,
                    codigo_tipo_articulo, codigo_cliente, nombre_cliente, codigo_pais, codigo_divisa_cliente,
                    codigo_producto, nombre_producto, codigo_calibre, nombre_calibre, npalet,
                    tipo_palet, nombre_tipo_palet, ncajas,peso_neto, unidad_peso, precio_comercial,
                    codigo_divisa, liquidado_son, estado_coop_liquidacion, liquidacion_agrupada_son, codigo_grupo_liquidacion,
                    num_liq, fra_liq, fra_liq_complementa, carta_liq, registro_liq, importe_liq, importe_iva_liq, tipo_iva_liq, 
                    fecha_liq, fecha_cambio_liq, cambio_liq, fecha_sc_liq, porcent_iva_liq, importe2_iva_liq, cobrado_son,
                    pagado_son, valor_mercancia, valor_confeccion, numero_salida_anecoop, salidalineaid) 
                    values ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}',
                    '{14}','{15}','{16}','{17}','{18}','{19}','{20}','{21}','{22}','{23:yyyy-MM-dd}','{24}','{25}','{26}',
                    '{27}','{28}','{29}','{30}','{31}','{32}','{33}','{34}','{35}','{36}','{37}',
                    '{38}','{39}','{40}','{41}','{42}','{43}','{44}','{45}','{46}','{47}','{48}',
                    '{49}','{50}','{51}','{52}','{53}','{54}','{55}','{56}','{57}','{58}','{59}',
                    '{60}','{61}','{62}','{63}','{64}','{65}','{66}','{67}','{68}','{69}','{70}',
                    '{71}','{72}','{73}','{74:yyyy-MM-dd}','{75:yyyy-MM-dd}','{76}','{77:yyyy-MM-dd}',
                    '{78}','{79}','{80}','{81}','{82}','{83}','{84}','{85}')
                    ON DUPLICATE KEY UPDATE
                    pdls_id='{1}',
                    periodo='{4}', codigo_cooperativa_gestora='{5}', nombre_gestora='{6}', codigo_cooperativa_carga='{7}', 
                    nombre_carga='{8}', pto_carga='{9}', nombre_pto_carga='{10}', 
                    nlinea_salida_cooperativa='{12}', n_pedido_aneccop='{13}', n_pedido='{14}', n_linea='{15}',
                    tipo_expediente='{16}', estado_coop_expediente='{17}', expediente_reenviado='{18}', codigo_delegacion='{19}',
                    nombre_delegacion='{20}', codigo_area='{21}', nombre_area='{22}', fecha_salida='{23:yyyy-MM-dd}', matricula='{24}', tipo_vehiculo='{25}',
                    categoria='{26}', nombre_categoria='{27}', codigo_confeccion='{28}', nombre_confeccion='{29}', 
                    codigo_uventa='{30}', nombre_uventa='{31}', codigo_etrasnsporte='{32}', nombre_etransporte='{33}', codigo_modelo='{34}',
                    nombre_modelo='{35}', codigo_material='{36}', nombre_material='{37}', codigo_marca='{38}', nombre_marca='{39}',
                    codigo_variedad='{40}', nombre_variedad='{41}', codigo_articulo='{42}', nombre_articulo='{43}', ean13='{44}',
                    codigo_tipo_articulo='{45}', codigo_cliente='{46}', nombre_cliente='{47}', codigo_pais='{48}', codigo_divisa_cliente='{49}',
                    codigo_producto='{50}', nombre_producto='{51}', codigo_calibre='{52}', nombre_calibre='{53}', npalet='{54}',
                    tipo_palet='{55}', nombre_tipo_palet='{56}', ncajas='{57}',peso_neto='{58}', unidad_peso='{59}', precio_comercial='{60}',
                    codigo_divisa='{61}', liquidado_son='{62}', estado_coop_liquidacion='{63}', liquidacion_agrupada_son='{64}', codigo_grupo_liquidacion='{65}',
                    num_liq='{66}', fra_liq='{67}', fra_liq_complementa='{68}', carta_liq='{69}', registro_liq='{70}', importe_liq='{71}', importe_iva_liq='{72}', tipo_iva_liq='{73}', 
                    fecha_liq='{74:yyyy-MM-dd}', fecha_cambio_liq='{75:yyyy-MM-dd}', cambio_liq='{76}', fecha_sc_liq='{77:yyyy-MM-dd}', porcent_iva_liq='{78}', importe2_iva_liq='{79}', cobrado_son='{80}',
                    pagado_son='{81}', valor_mercancia='{82}', valor_confeccion='{83}', numero_salida_anecoop='{84}', salidalineaid='{85}'";
                sql = String.Format(sql, expediente_id, pdls_id, linea_expediente, codigo_campanya,
                    periodo, codigo_cooperativa_gestora.ToString().Replace(",","."), nombre_gestora, codigo_cooperativa_carga,
                    nombre_carga, pto_carga, nombre_pto_carga, numero_salida_cooperativa,
                    nlinea_salida_cooperativa, n_pedido_aneccop.ToString().Replace(",", "."), n_pedido.ToString().Replace(",", "."), n_linea.ToString().Replace(",", "."),
                    tipo_expediente, estado_coop_expediente, expediente_reenviado, codigo_delegacion,
                    nombre_delegacion, codigo_area, nombre_area, fecha_salida, matricula, tipo_vehiculo,
                    categoria, nombre_categoria, codigo_confeccion, nombre_confeccion,
                    codigo_uventa, nombre_uventa, codigo_etransporte, nombre_etransporte, codigo_modelo,
                    nombre_modelo, codigo_material, nombre_material, codigo_marca, nombre_marca,
                    codigo_variedad, nombre_variedad, codigo_articulo, nombre_articulo, ean13,
                    codigo_tipo_articulo, codigo_cliente, nombre_cliente, codigo_pais, codigo_divisa_cliente,
                    codigo_producto, nombre_producto, codigo_calibre, nombre_calibre, npalet.ToString().Replace(",", "."),
                    tipo_palet, nombre_tipo_palet, ncajas.ToString().Replace(",", "."), peso_neto.ToString().Replace(",", "."), unidad_peso, precio_comercial.ToString().Replace(",", "."),
                    codigo_divisa, liquidacion_son, estado_coop_liquidacion, liquidacion_agrupada_son, codigo_grupo_liquidacion,
                    num_liq.ToString().Replace(",", "."), fra_liq, fra_liq_complementaria, carta_liq, registro_liq, importe_liq.ToString().Replace(",", "."), importe_iva_liq.ToString().Replace(",", "."), tipo_iva_liq,
                    fecha_liq, fecha_cambio_liq, cambio_liq.ToString().Replace(",", "."), fecha_sc_liq, porcentaje_iva_liq.ToString().Replace(",", "."), importe2_iva_liq.ToString().Replace(",", "."), cobrado_son,
                    pagado_son, valor_mercancia.ToString().Replace(",", "."), valor_confeccion.ToString().Replace(",", "."), numero_salida_aneccop, salida_linea_id);
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
                DescargaPedidosResultExpedienteEXPEDIENTE_PAGO[] pagos = exp.EXPEDIENTE_PAGO;
                if (pagos != null)
                {
                    foreach (DescargaPedidosResultExpedienteEXPEDIENTE_PAGO pago in pagos)
                    {
                        string expediente_pagoid = pago.EXPEDIENTE_PAGOID; 
                        string expedientep_id = pago.EXPEDIENTE_ID; 
                        string tipo_pago = pago.TIPO_PAGO; 
                        string num_factura = pago.NUM_FACTURA;
                        DateTime fecha_factura = pago.FECHA_FACTURA;
                        string num_liquidacion = pago.NUM_LIQUIDACION;
                        decimal importe = pago.IMPORTE;
                        DateTime fecha_pago = pago.FECHA_PAGO;
                        DateTime fecha_pago_sc = pago.FECHA_PAGO_SC;
                        string estado = pago.ESTADO;
                        
                        // Grabación en base de datos
                        sql = @"INSERT INTO anecoop_pago(expediente_id,expediente_pagoid,tipo_pago,num_factura,fecha_factura,num_liquidacion,importe,fecha_pago,estado) 
                        values ('{0}','{1}','{2}','{3}','{4:yyyy-MM-dd}','{5}','{6}','{7:yyyy-MM-dd}','{8}')
                        ON DUPLICATE KEY UPDATE
                        tipo_pago='{2}',num_factura='{3}',fecha_factura='{4:yyyy-MM-dd}',num_liquidacion='{5}',importe='{6}',fecha_pago='{7:yyyy-MM-dd}',estado='{8}'
                        ";
                        sql = String.Format(sql, expediente_id, expediente_pagoid, tipo_pago, num_factura, fecha_factura, num_liquidacion, importe.ToString().Replace(",","."), fecha_pago, estado);
                        cmd.CommandText = sql;
                        cmd.ExecuteNonQuery();
                    }
                }

                DescargaPedidosResultExpedienteEXPEDIENTE_COBRO[] cobros = exp.EXPEDIENTE_COBRO;
                if (cobros != null)
                {
                    foreach (DescargaPedidosResultExpedienteEXPEDIENTE_COBRO cobro in cobros)
                    {
                        string expediente_cobroid = cobro.EXPEDIENTE_COBROID;
                        string expedientec_id = cobro.EXPEDIENTE_ID;
                        string numero_remesa = cobro.NUMERO_REMESA;
                        decimal importe_cambio = cobro.IMPORTE_CAMBIO;
                        DateTime fecha_cambio = cobro.FECHA_CAMBIO;
                        string estado = cobro.ESTADO;

                        // Grabación en base de datos
                        sql = @"INSERT INTO anecoop_cobro(expediente_id,expediente_cobroid,numero_remesa,importe_cambio,fecha_cambio,estado) 
                        values ('{0}','{1}','{2}','{3}','{4:yyyy-MM-dd}','{5}')
                        ON DUPLICATE KEY UPDATE
                        numero_remesa='{2}',importe_cambio='{3}',fecha_cambio='{4:yyyy-MM-dd}',estado='{5}'
                        ";
                        sql = String.Format(sql, expediente_id, expediente_cobroid, numero_remesa, importe_cambio.ToString().Replace(",","."), fecha_cambio, estado);
                        cmd.CommandText = sql;
                        cmd.ExecuteNonQuery();
                        
                    }
                }


                if (informa) Console.Write("\rProcesado registro {0} de {1}           ", ++numReg, totReg);
                

            }
            conn.Close();

            if (informa) Console.WriteLine("");
            if (informa) Console.WriteLine("Proceso finalizado, pulse INTRO para cerrar.");
            if (informa) Console.ReadLine();

            //-- Creamos el fichero chivato en el mismo directorio.
            string f = Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase);
            f = f.Replace("file:\\", ""); // elimina URI format
            f = String.Format("{0}\\aneccop.z", f);
            //
            TextWriter tw = new StreamWriter(f);
            tw.WriteLine("ANECOOP FINALIZADO: {0:dd/MM/yyyy hh:mm:ss}", DateTime.Now);
            tw.Close();
        }
    }
}
