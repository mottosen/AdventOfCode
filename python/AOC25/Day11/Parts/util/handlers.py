import re


def line_2_connection(line):
    match = re.match(r"(\w+): (.*)", line)

    device = match.group(1)
    outputs = re.findall(r"\w+", match.group(2))

    return device, {"outputs": outputs, "paths": 0}


def handle_device(connections, device):
    device_info = connections[device]

    # we already know the solution for devices seen
    if device_info["paths"] > 0:
        return device_info["paths"]

    # if we haven't seen the device before
    else:
        device_paths = 0
        # a device paths is a sum of output device paths
        for output in device_info["outputs"]:
            # only one step to the 'out' device
            if output == "out":
                device_paths += 1

            # recursively solve connected device
            else:
                device_paths += handle_device(connections, output)

        return device_paths
