from pathlib import Path

from Parts.util.handlers import handle_device, line_2_connection


class Part1:
    def __init__(self, input_file: Path):
        with open(input_file, "r") as f:
            self.input_data = [line_2_connection(line) for line in f.readlines()]

        self.connections = {k: v for k, v in self.input_data}

    def get_day_solution(self):
        return str(handle_device(self.connections, "you"))
