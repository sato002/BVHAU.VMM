using Newtonsoft.Json;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BVHAU.VMM
{
    internal class Program
    {
        static string FILE_NAME = "vms.json";

        static void Main(string[] args)
        {
            try
            {
                var config = LoadConfig();
                Console.WriteLine("=== Menu, type 1|2|3 to start ===");
                Console.WriteLine("1: Create snapshot");
                Console.WriteLine("2: Delete snapshot");
                Console.WriteLine("3: Run snapshot");
                string input = Console.ReadLine();
                int option = int.Parse(input);
                switch (option)
                {
                    case 1:
                        CreateSnapshot(config);
                        break;
                    case 2:
                        DeleteSnapshot(config);
                        break;
                    case 3:
                        RevertSnapshot(config);
                        break;
                    default:
                        throw new Exception("type 1|2|3 to start");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            Console.ReadLine();
        }

        static UserConfig LoadConfig()
        {
            Console.WriteLine("=== Danh sach may ao - snapshot ===");
            UserConfig userConfig = null;


            if (File.Exists(FILE_NAME))
            {
                userConfig = JsonConvert.DeserializeObject<UserConfig>(File.ReadAllText(FILE_NAME));
                if (userConfig != null && userConfig.Vms != null && userConfig.Vms.Count > 0)
                {
                    for (int i = 0; i < userConfig.Vms.Count; i++)
                    {
                        var vm = userConfig.Vms[i];
                        Console.WriteLine($"{i + 1}. {vm.VMXPath} - {vm.SnapshotName}");
                    }
                }
                else
                {
                    throw new Exception("File config không hợp lệ hoặc không có vm nào");
                }
            }
            else
            {
                throw new Exception("Không tìm thấy file vms.json");
            }

            return userConfig;
        }

        static void RevertSnapshot(UserConfig config)
        {
            for (int i = 0; i < config.Vms.Count; i++)
            {
                var vm = config.Vms[i];
                Console.WriteLine($"Revert snapshot {i + 1}: {vm.SnapshotName}");
                VMRUN_Revert(config.VmrunPath, vm);
                Thread.Sleep(config.IntervalTimeMs);
                VMRUN_Start(config.VmrunPath, vm);
                Thread.Sleep(config.IntervalTimeMs);
            }
        }

        static void CreateSnapshot(UserConfig config)
        {
            for (int i = 0; i < config.Vms.Count; i++)
            {
                var vm = config.Vms[i];
                Console.WriteLine($"Create Snapshot {i + 1}: {vm.SnapshotName}");
                VMRUN_CreateSnapshot(config.VmrunPath, vm);
                Thread.Sleep(config.IntervalTimeMs);
            }
        }

        static void DeleteSnapshot(UserConfig config)
        {
            for (int i = 0; i < config.Vms.Count; i++)
            {
                var vm = config.Vms[i];
                Console.WriteLine($"Delete Snapshot {i + 1}: {vm.SnapshotName}");
                VMRUN_DeleteSnapshot(config.VmrunPath, vm);
                Thread.Sleep(config.IntervalTimeMs);
            }
        }

        static void VMRUN_CreateSnapshot(string vmrunPath, VM vm)
        {
            ProcessStartInfo psi = new ProcessStartInfo
            {
                FileName = vmrunPath,
                Arguments = $"snapshot \"{vm.VMXPath}\" \"{vm.SnapshotName}\"",
                UseShellExecute = true // Set this to true to use the operating system's default shell for launching the executable
            };

            try
            {
                // Start the process
                Process.Start(psi);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }

        static void VMRUN_DeleteSnapshot(string vmrunPath, VM vm)
        {
            ProcessStartInfo psi = new ProcessStartInfo
            {
                FileName = vmrunPath,
                Arguments = $"deleteSnapshot \"{vm.VMXPath}\" \"{vm.SnapshotName}\"",
                UseShellExecute = true // Set this to true to use the operating system's default shell for launching the executable
            };

            try
            {
                // Start the process
                Process.Start(psi);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }

        static void VMRUN_Revert(string vmrunPath, VM vm)
        {
            ProcessStartInfo psi = new ProcessStartInfo
            {
                FileName = vmrunPath,
                Arguments = $"revertToSnapshot \"{vm.VMXPath}\" \"{vm.SnapshotName}\"",
                UseShellExecute = true // Set this to true to use the operating system's default shell for launching the executable
            };

            try
            {
                // Start the process
                Process.Start(psi);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }

        static void VMRUN_Start(string vmrunPath, VM vm)
        {
            ProcessStartInfo psi = new ProcessStartInfo
            {
                FileName = vmrunPath,
                Arguments = $"start \"{vm.VMXPath}\"",
                UseShellExecute = true // Set this to true to use the operating system's default shell for launching the executable
            };

            try
            {
                // Start the process
                Process.Start(psi);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }
    }
}
